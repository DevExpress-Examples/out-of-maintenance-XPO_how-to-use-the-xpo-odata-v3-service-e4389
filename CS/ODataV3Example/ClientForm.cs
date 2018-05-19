using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data.Services.Client;
using System.Linq;
using System.Collections;
using System.IO;
using DevExpress.XtraGrid.Views.Base;
using System.Drawing;
using System.Xml;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Csdl;
using Microsoft.Data.Edm.Validation;
using System.Net;
using Microsoft.Data.Edm.Annotations;
using System.ComponentModel;
using DevExpress.Xpo;
using System.Reflection;
using System.Linq.Expressions;
using Microsoft.Data.Edm.Values;

namespace ODataV3Example.Northwind {
    public partial class ClientForm : Form {
        XpoExampleContext context;
        string authCookie = string.Empty;
        readonly Dictionary<string, DataServiceQuery> entities = new Dictionary<string, DataServiceQuery>();
        readonly static Uri serviceUri = new Uri("http://localhost:50293/XpoOdataService.svc/");
        readonly Dictionary<string, Dictionary<string, XpoAnnotation>> annotations = new Dictionary<string, Dictionary<string, XpoAnnotation>>();
        readonly Dictionary<Type, Dictionary<string, Func<object, object>>> properyValuesGetter = new Dictionary<Type, Dictionary<string, Func<object, object>>>();
        IList listObjects;
        IList ListObjects {
            get { return listObjects; }
            set {
                if(listObjects != null) {
                    foreach(INotifyPropertyChanged entity in listObjects)
                        entity.PropertyChanged -= entity_PropertyChanged;
                }
                listObjects = value;
                if(listObjects != null) {
                    foreach(INotifyPropertyChanged entity in listObjects)
                        entity.PropertyChanged += entity_PropertyChanged;
                }
                gclODataV3ServiceMain.DataSource = listObjects;
                gclODataV3ServiceMain.MainView.RefreshData();
                gclODataV3ServiceMain.MainView.PopulateColumns();
            }
        }
        public ClientForm() {
            InitializeComponent();
            CreateContext();
            InitializeEntityList();
            use_Click(null, null);
            GetAnnotation();
        }

        private void InitializeEntityList() {
            entities.Add("Categories", context.Categories);
            entities.Add("Employees", context.Employees);
            entities.Add("Products", context.Products);
            entities.Add("Orders", context.Orders);
            entities.Add("Shippers", context.Shippers);
            entities.Add("Suppliers", context.Suppliers);
            entities.Add("Customers", context.Customers);
            entityToShow.Items.AddRange(entities.Keys.ToArray());
            entityToShow.SelectedIndex = 0;
        }

        private void CreateContext() {
            context = new XpoExampleContext(serviceUri);
            context.SendingRequest += context_SendingRequest;
            context.MergeOption = MergeOption.OverwriteChanges;
        }

        void context_SendingRequest(object sender, SendingRequestEventArgs e) {
            e.RequestHeaders.Add("userCookie", authCookie);
        }

        private void Authorize(object sender, EventArgs e) {
            try {
                UriOperationParameter param1 = new UriOperationParameter("name", "Davolio");
                UriOperationParameter param2 = new UriOperationParameter("password", "abracadabra");
                string res = context.Execute<string>(new Uri("GetAuthorizationCookie", UriKind.Relative), "GET", true, param1, param2).Single();
                authCookie = res;
                entityToShow.Enabled = true;
                entityToShow_SelectedIndexChanged(null, null);
                AuthorizeButton.Enabled = false;
            } catch(Exception ex) {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void gvlODataV3Service_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e) {
            pictureBox.Image = null;
            if(ListObjects == null || !(ListObjects[0] is Categories)) return;
            ColumnView view = gclODataV3ServiceMain.FocusedView as ColumnView;
            if(view.Columns.Count < 1 || view.FocusedRowHandle < 0) return;
            object value = view.GetRowCellValue(view.FocusedRowHandle, view.Columns["CategoryID"]);
            if(value == null) {
                pictureBox.Image = null;
            } else {
                int categoryID = (int)value;
                DataServiceStreamResponse resp = context.GetReadStream(context.Categories.Where(i => i.CategoryID == categoryID).Single(), "StreamPicture", new DataServiceRequestArgs());
                pictureBox.Image = Image.FromStream(resp.Stream);
            }
        }

        private void entityToShow_SelectedIndexChanged(object sender, EventArgs e) {
            if(entityToShow.SelectedIndex < 0) return;
            if(string.IsNullOrEmpty(authCookie)) return;
            try {
                context.SaveChanges();
                DataServiceQuery queue = entities[entityToShow.SelectedItem.ToString()];
                List<object> ds = new List<object>();
                ds.AddRange((IEnumerable<object>)queue.Execute());
                ListObjects = ds;
                if(ListObjects[0] is Categories) {
                    categoryInteceptor.Enabled = true;
                    gvlODataV3Service_FocusedRowChanged(null, null);
                } else {
                    categoryInteceptor.Enabled = false;
                    pictureBox.Image = null;
                }
                if(ListObjects[0] is Products) {
                    checkRemove.Enabled = true;
                } else {
                    checkRemove.Enabled = false;
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        private void use_Click(object sender, EventArgs e) {
            UriOperationParameter param1 = new UriOperationParameter("state", true);
            context.Execute(new Uri("SetCategoriesFiltering", UriKind.Relative), "GET", param1);
            entityToShow_SelectedIndexChanged(null, null);
        }

        private void dontUse_Click(object sender, EventArgs e) {
            UriOperationParameter param1 = new UriOperationParameter("state", false);
            context.Execute(new Uri("SetCategoriesFiltering", UriKind.Relative), "GET", param1);
            entityToShow_SelectedIndexChanged(null, null);
        }

        void GetAnnotation() {
            Uri metadataUri = context.GetMetadataUri();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(metadataUri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            stream.Flush();
            string metadata = sr.ReadToEnd();

            IEdmModel annotatedModel;
            IEnumerable<EdmError> errors;
            XmlReader xmlReader = XmlReader.Create(new StringReader(metadata));
            bool parsed = EdmxReader.TryParse(xmlReader, out annotatedModel, out errors);

            foreach(IEdmVocabularyAnnotation annotation in annotatedModel.VocabularyAnnotations) {
                if(annotation.Term.TermKind != EdmTermKind.Value) continue;
                IEdmValueAnnotation valueAnnotation = (IEdmValueAnnotation)annotation;
                IEdmProperty property = (IEdmProperty)valueAnnotation.Target;
                IEdmTerm term = valueAnnotation.Term;
                string entityTypeName = ((IEdmSchemaType)property.DeclaringType).Name;
                if(!annotations.ContainsKey(entityTypeName))
                    annotations.Add(entityTypeName, new Dictionary<string, XpoAnnotation>());
                if(!annotations[entityTypeName].ContainsKey(property.Name))
                    annotations[entityTypeName][property.Name] = new XpoAnnotation();
                switch(term.Name) {
                    case "Size":
                        annotations[entityTypeName][property.Name].Size = (int)((IEdmIntegerValue)valueAnnotation.Value).Value;
                        break;
                    case "ReadOnly":
                        annotations[entityTypeName][property.Name].ReadOnly = ((IEdmBooleanValue)valueAnnotation.Value).Value;
                        break;
                }
            }
        }

        void entity_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            Dictionary<string, XpoAnnotation> propertiesAnnotattion;
            if(!annotations.TryGetValue(sender.GetType().Name, out propertiesAnnotattion))
                return;
            XpoAnnotation annotation;
            if(!propertiesAnnotattion.TryGetValue(e.PropertyName, out annotation))
                return;
            if(annotation.ReadOnly) {
                throw new ArgumentException(e.PropertyName);
            }
            object value = GetProperyValue(sender, e.PropertyName);
            if(annotation.Size > 0) {
                if(value.GetType() == typeof(byte[]) && ((byte[])value).Length > annotation.Size)
                    throw new ArgumentException(e.PropertyName);
                if(value.GetType() == typeof(string) && ((string)value).Length > annotation.Size)
                    throw new ArgumentException(e.PropertyName);
            }
        }

        public object GetProperyValue(object entity, string propertyName) {
            if(entity == null) return null;
            Type entityType = entity.GetType();
            Dictionary<string, Func<object, object>> properyGetters;
            if(!properyValuesGetter.TryGetValue(entity.GetType(), out properyGetters))
                properyValuesGetter.Add(entity.GetType(), new Dictionary<string, Func<object, object>>());
            Func<object, object> func;
            if(!properyValuesGetter[entity.GetType()].TryGetValue(propertyName, out func)) {
                Expression body = null;
                ParameterExpression entityParameter = Expression.Parameter(typeof(object), "e");
                Expression instance = Expression.Convert(entityParameter, entityType);
                MemberInfo member = entityType.GetMember(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).Single();
                body = Expression.MakeMemberAccess(instance, member);
                if(body == null)
                    throw new InvalidOperationException(propertyName);
                if(body.Type != typeof(object)) {
                    body = Expression.Convert(body, typeof(object));
                }
                func = Expression.Lambda<Func<object, object>>(body, entityParameter).Compile();
                properyValuesGetter[entity.GetType()][propertyName] = func;
            }
            return func(entity);
        }

        private void gvlODataV3Service_CellValueChanged(object sender, CellValueChangedEventArgs e) {
            ColumnView view = gclODataV3ServiceMain.FocusedView as ColumnView;
            object entity = view.GetRow(e.RowHandle);
            context.UpdateObject(entity);
        }

        private void checkRemove_Click(object sender, EventArgs e) {
            if(ListObjects == null || !(ListObjects[0] is Products)) return;
            ColumnView view = gclODataV3ServiceMain.FocusedView as ColumnView;
            if(view.Columns.Count < 1 || view.FocusedRowHandle < 0) return;
            int productId = (int)view.GetRowCellValue(view.FocusedRowHandle, view.Columns["ProductID"]);
            Uri actionUri = new Uri(string.Format("{1}({0})/CanRemove", productId, context.Products.RequestUri.AbsoluteUri));
            bool result = context.Execute<bool>(actionUri, "POST", true).Single();
            if(result)
                MessageBox.Show("Can remove");
            else
                MessageBox.Show("Can't remove");
        }
    }

}
