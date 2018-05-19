using System;
using System.Collections.Generic;
using System.Data.Services;
using Northwind;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System.ServiceModel;
using System.IO;
using System.Web;
using System.Data.Services.Common;
using System.Linq.Expressions;
using System.ServiceModel.Web;
using System.Linq;
using System.Web.Security;

namespace XpoODataExample {
    public class XpoODataContext : XpoContext {
        public bool CategoriesFiltering = false;
        public XpoODataContext(string s1, string s2, IDataLayer dataLayer)
            : base(s1, s2, dataLayer) {
        }
        [Action]
        public bool CanRemove(Products product) {
            return !(new XPQuery<Suppliers>(product.Session)).Any(s => s.Products.Contains(product));
        }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class XpoODataExampleService : XpoDataServiceV3 {
        static readonly XpoODataContext context = new XpoODataContext("XpoExampleContext", "Northwind", CreateDataLayer());

        public XpoODataExampleService() : base(context, context) { }

        static IDataLayer CreateDataLayer() {
            DevExpress.Xpo.Metadata.XPDictionary dict = new DevExpress.Xpo.Metadata.ReflectionDictionary();
            // Initialize the XPO dictionary. 
            dict.GetDataStoreSchema(typeof(Orders).Assembly);
            InMemoryDataStore store = new InMemoryDataStore(AutoCreateOption.SchemaOnly);
            string DBFileName = DevExpress.Utils.FilesHelper.FindingFileName(HttpRuntime.AppDomainAppPath, "App_Data\\nwind.xml");
            if(DBFileName != "")
                store.ReadXml(DBFileName);
            IDataLayer dataLayer = new ThreadSafeDataLayer(dict, store);
            XpoDefault.DataLayer = dataLayer;
            XpoDefault.Session = null;
            return dataLayer;
        }
        protected override void OnStartProcessingRequest(ProcessRequestArgs args) {
            base.OnStartProcessingRequest(args);
        }
        public static void InitializeService(DataServiceConfiguration config) {
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.DataServiceBehavior.AcceptAnyAllRequests = true;
            config.UseVerboseErrors = true;
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
            config.SetServiceActionAccessRule("*", ServiceActionRights.Invoke);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
            config.DataServiceBehavior.AcceptProjectionRequests = true;
            config.DataServiceBehavior.AcceptCountRequests = true;
            config.AnnotationsBuilder = CreateAnnotationsBuilder(() => context);
            config.DataServiceBehavior.AcceptReplaceFunctionInQuery = true;
            config.DataServiceBehavior.AcceptSpatialLiteralsInQuery = true;
            config.DisableValidationOnMetadataWrite = true;
        }

        [QueryInterceptor("Categories")]
        public Expression<Func<Categories, bool>> OnQueryActors() {
            if(context.CategoriesFiltering)
                return o => o.CategoryName.Length > 12;
            else
                return o => true;
        }

        [ChangeInterceptor("Products")]
        public void OnChangeProducts(Products product, UpdateOperations operations) {
            if (Equals(operations, UpdateOperations.Change)) {
                Products productInBase = GetEntityUpdated<Products>(product);
                if (productInBase.ProductName != product.ProductName) {
                    throw new DataServiceException(400, string.Format("A {0} cannot be modified.", product.ToString()));
                }
            } else if (Equals(operations, UpdateOperations.Delete)) {
                throw new DataServiceException(400, "Products cannot be deleted.");
            }
        }

        public override object Authenticate(ProcessRequestArgs args) {
            int oid;
            try {
                string authCookie = args.OperationContext.RequestHeaders["userCookie"];
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie);
                oid = int.Parse(ticket.UserData);
            } catch {
                return null;
            }
            using(UnitOfWork uow = new UnitOfWork(Context.ObjectLayer)) {
                Employees cont = uow.GetObjectByKey<Employees>(oid);
                return cont != null ? "ok" : ":(";
            }
        }

        public override LambdaExpression GetQueryInterceptor(Type entityType, object token) {
            if(token == null) throw new DataServiceException(401, "Unauthorized access.");
            if((string)token == "ok")
                return (Expression<Func<object, bool>>)(o => true);
            else {
                return (Expression<Func<object, bool>>)(o => false);
            }
        }

        [WebGet]
        public string GetAuthorizationCookie(string name, string password) {
            using(UnitOfWork uow = new UnitOfWork(Context.ObjectLayer)) {
                List<Employees> contacts = new XPQuery<Employees>(uow).Where(i => i.LastName == name).ToList();
                Employees contact = contacts.SingleOrDefault();

                if(contact == null) return string.Empty;
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    contact.LastName,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    true,
                    contact.EmployeeID.ToString());

                string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                return encryptedTicket;
            }
        }

        [WebGet]
        public void SetCategoriesFiltering(bool state) {
            context.CategoriesFiltering = state;
        }
    }
}
