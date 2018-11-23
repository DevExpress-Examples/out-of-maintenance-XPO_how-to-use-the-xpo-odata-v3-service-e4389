Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Data.Services.Client
Imports System.Linq
Imports System.Collections
Imports System.IO
Imports DevExpress.XtraGrid.Views.Base
Imports System.Drawing
Imports System.Xml
Imports Microsoft.Data.Edm
Imports Microsoft.Data.Edm.Csdl
Imports Microsoft.Data.Edm.Validation
Imports System.Net
Imports Microsoft.Data.Edm.Annotations
Imports System.ComponentModel
Imports DevExpress.Xpo
Imports System.Reflection
Imports System.Linq.Expressions
Imports Microsoft.Data.Edm.Values

Namespace ODataV3Example.Northwind
	Partial Public Class ClientForm
		Inherits Form
		Private context As XpoExampleContext
		Private authCookie As String = String.Empty
		Private ReadOnly entities As New Dictionary(Of String, DataServiceQuery)()
		Private ReadOnly Shared serviceUri As New Uri("http://localhost:50293/XpoOdataService.svc/")
		Private ReadOnly annotations As New Dictionary(Of String, Dictionary(Of String, XpoAnnotation))()
		Private ReadOnly properyValuesGetter As New Dictionary(Of Type, Dictionary(Of String, Func(Of Object, Object)))()
		Private listObjects_Renamed As IList
		Private Property ListObjects() As IList
			Get
				Return listObjects_Renamed
			End Get
			Set(ByVal value As IList)
				If listObjects_Renamed IsNot Nothing Then
					For Each entity As INotifyPropertyChanged In listObjects_Renamed
						RemoveHandler entity.PropertyChanged, AddressOf entity_PropertyChanged
					Next entity
				End If
				listObjects_Renamed = value
				If listObjects_Renamed IsNot Nothing Then
					For Each entity As INotifyPropertyChanged In listObjects_Renamed
						AddHandler entity.PropertyChanged, AddressOf entity_PropertyChanged
					Next entity
				End If
				gclODataV3ServiceMain.DataSource = listObjects_Renamed
				gclODataV3ServiceMain.MainView.RefreshData()
				gclODataV3ServiceMain.MainView.PopulateColumns()
			End Set
		End Property
		Public Sub New()
			InitializeComponent()
			CreateContext()
			InitializeEntityList()
			use_Click(Nothing, Nothing)
			GetAnnotation()
		End Sub

		Private Sub InitializeEntityList()
			entities.Add("Categories", context.Categories)
			entities.Add("Employees", context.Employees)
			entities.Add("Products", context.Products)
			entities.Add("Orders", context.Orders)
			entities.Add("Shippers", context.Shippers)
			entities.Add("Suppliers", context.Suppliers)
			entities.Add("Customers", context.Customers)
			entityToShow.Items.AddRange(entities.Keys.ToArray())
			entityToShow.SelectedIndex = 0
		End Sub

		Private Sub CreateContext()
			context = New XpoExampleContext(serviceUri)
			AddHandler context.SendingRequest, AddressOf context_SendingRequest
			context.MergeOption = MergeOption.OverwriteChanges
		End Sub

		Private Sub context_SendingRequest(ByVal sender As Object, ByVal e As SendingRequestEventArgs)
			e.RequestHeaders.Add("userCookie", authCookie)
		End Sub

		Private Sub Authorize(ByVal sender As Object, ByVal e As EventArgs) Handles AuthorizeButton.Click
			Try
				Dim param1 As New UriOperationParameter("name", "Davolio")
				Dim param2 As New UriOperationParameter("password", "abracadabra")
				Dim res As String = context.Execute(Of String)(New Uri("GetAuthorizationCookie", UriKind.Relative), "GET", True, param1, param2).Single()
				authCookie = res
				entityToShow.Enabled = True
				entityToShow_SelectedIndexChanged(Nothing, Nothing)
				AuthorizeButton.Enabled = False
			Catch ex As Exception
				MessageBox.Show(ex.Message.ToString())
			End Try
		End Sub

		Private Sub gvlODataV3Service_FocusedRowChanged(ByVal sender As Object, ByVal e As FocusedRowChangedEventArgs) Handles gvlODataV3Service.FocusedRowChanged
			pictureBox.Image = Nothing
			If ListObjects Is Nothing OrElse Not(TypeOf ListObjects(0) Is Categories) Then
				Return
			End If
			Dim view As ColumnView = TryCast(gclODataV3ServiceMain.FocusedView, ColumnView)
			If view.Columns.Count < 1 OrElse view.FocusedRowHandle < 0 Then
				Return
			End If
			Dim value As Object = view.GetRowCellValue(view.FocusedRowHandle, view.Columns("CategoryID"))
			If value Is Nothing Then
				pictureBox.Image = Nothing
			Else
				Dim categoryID As Integer = CInt(Fix(value))
				Dim resp As DataServiceStreamResponse = context.GetReadStream(context.Categories.Where(Function(i) i.CategoryID = categoryID).Single(), "StreamPicture", New DataServiceRequestArgs())
				pictureBox.Image = Image.FromStream(resp.Stream)
			End If
		End Sub

		Private Sub entityToShow_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles entityToShow.SelectedIndexChanged
			If entityToShow.SelectedIndex < 0 Then
				Return
			End If
			If String.IsNullOrEmpty(authCookie) Then
				Return
			End If
			Try
				context.SaveChanges()
				Dim queue As DataServiceQuery = entities(entityToShow.SelectedItem.ToString())
				Dim ds As New List(Of Object)()
				ds.AddRange(CType(queue.Execute(), IEnumerable(Of Object)))
				ListObjects = ds
				If TypeOf ListObjects(0) Is Categories Then
					categoryInteceptor.Enabled = True
					gvlODataV3Service_FocusedRowChanged(Nothing, Nothing)
				Else
					categoryInteceptor.Enabled = False
					pictureBox.Image = Nothing
				End If
				If TypeOf ListObjects(0) Is Products Then
					checkRemove.Enabled = True
				Else
					checkRemove.Enabled = False
				End If
			Catch ex As Exception
				MessageBox.Show(ex.ToString())
			End Try
		End Sub

		Private Sub use_Click(ByVal sender As Object, ByVal e As EventArgs) Handles use.Click
			Dim param1 As New UriOperationParameter("state", True)
			context.Execute(New Uri("SetCategoriesFiltering", UriKind.Relative), "GET", param1)
			entityToShow_SelectedIndexChanged(Nothing, Nothing)
		End Sub

		Private Sub dontUse_Click(ByVal sender As Object, ByVal e As EventArgs) Handles dontUse.Click
			Dim param1 As New UriOperationParameter("state", False)
			context.Execute(New Uri("SetCategoriesFiltering", UriKind.Relative), "GET", param1)
			entityToShow_SelectedIndexChanged(Nothing, Nothing)
		End Sub

		Private Sub GetAnnotation()
			Dim metadataUri As Uri = context.GetMetadataUri()
			Dim request As HttpWebRequest = CType(WebRequest.Create(metadataUri), HttpWebRequest)
			Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
			Dim stream As Stream = response.GetResponseStream()
			Dim sr As New StreamReader(stream)
			stream.Flush()
			Dim metadata As String = sr.ReadToEnd()

			Dim annotatedModel As IEdmModel
			Dim errors As IEnumerable(Of EdmError)
			Dim xmlReader As XmlReader = XmlReader.Create(New StringReader(metadata))
			Dim parsed As Boolean = EdmxReader.TryParse(xmlReader, annotatedModel, errors)

			For Each annotation As IEdmVocabularyAnnotation In annotatedModel.VocabularyAnnotations
				If annotation.Term.TermKind <> EdmTermKind.Value Then
					Continue For
				End If
				Dim valueAnnotation As IEdmValueAnnotation = CType(annotation, IEdmValueAnnotation)
				Dim [property] As IEdmProperty = CType(valueAnnotation.Target, IEdmProperty)
				Dim term As IEdmTerm = valueAnnotation.Term
				Dim entityTypeName As String = (CType([property].DeclaringType, IEdmSchemaType)).Name
				If (Not annotations.ContainsKey(entityTypeName)) Then
					annotations.Add(entityTypeName, New Dictionary(Of String, XpoAnnotation)())
				End If
				If (Not annotations(entityTypeName).ContainsKey([property].Name)) Then
					annotations(entityTypeName)([property].Name) = New XpoAnnotation()
				End If
				Select Case term.Name
					Case "Size"
						annotations(entityTypeName)([property].Name).Size = CInt(Fix((CType(valueAnnotation.Value, IEdmIntegerValue)).Value))
					Case "ReadOnly"
						annotations(entityTypeName)([property].Name).ReadOnly = (CType(valueAnnotation.Value, IEdmBooleanValue)).Value
				End Select
			Next annotation
		End Sub

		Private Sub entity_PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs)
			Dim propertiesAnnotattion As Dictionary(Of String, XpoAnnotation)
			If (Not annotations.TryGetValue(sender.GetType().Name, propertiesAnnotattion)) Then
				Return
			End If
			Dim annotation As XpoAnnotation
			If (Not propertiesAnnotattion.TryGetValue(e.PropertyName, annotation)) Then
				Return
			End If
			If annotation.ReadOnly Then
				Throw New ArgumentException(e.PropertyName)
			End If
			Dim value As Object = GetProperyValue(sender, e.PropertyName)
			If annotation.Size > 0 Then
				If value.GetType() Is GetType(Byte()) AndAlso (CType(value, Byte())).Length > annotation.Size Then
					Throw New ArgumentException(e.PropertyName)
				End If
				If value.GetType() Is GetType(String) AndAlso (CStr(value)).Length > annotation.Size Then
					Throw New ArgumentException(e.PropertyName)
				End If
			End If
		End Sub

		Public Function GetProperyValue(ByVal entity As Object, ByVal propertyName As String) As Object
			If entity Is Nothing Then
				Return Nothing
			End If
			Dim entityType As Type = entity.GetType()
			Dim properyGetters As Dictionary(Of String, Func(Of Object, Object))
			If (Not properyValuesGetter.TryGetValue(entity.GetType(), properyGetters)) Then
				properyValuesGetter.Add(entity.GetType(), New Dictionary(Of String, Func(Of Object, Object))())
			End If
			Dim func As Func(Of Object, Object)
			If (Not properyValuesGetter(entity.GetType()).TryGetValue(propertyName, func)) Then
				Dim body As Expression = Nothing
				Dim entityParameter As ParameterExpression = Expression.Parameter(GetType(Object), "e")
				Dim instance As Expression = Expression.Convert(entityParameter, entityType)
				Dim member As MemberInfo = entityType.GetMember(propertyName, BindingFlags.Public Or BindingFlags.Instance Or BindingFlags.NonPublic).Single()
				body = Expression.MakeMemberAccess(instance, member)
				If body Is Nothing Then
					Throw New InvalidOperationException(propertyName)
				End If
				If body.Type IsNot GetType(Object) Then
					body = Expression.Convert(body, GetType(Object))
				End If
				func = Expression.Lambda(Of Func(Of Object, Object))(body, entityParameter).Compile()
				properyValuesGetter(entity.GetType())(propertyName) = func
			End If
			Return func(entity)
		End Function

		Private Sub gvlODataV3Service_CellValueChanged(ByVal sender As Object, ByVal e As CellValueChangedEventArgs) Handles gvlODataV3Service.CellValueChanged
			Dim view As ColumnView = TryCast(gclODataV3ServiceMain.FocusedView, ColumnView)
			Dim entity As Object = view.GetRow(e.RowHandle)
			context.UpdateObject(entity)
		End Sub

		Private Sub checkRemove_Click(ByVal sender As Object, ByVal e As EventArgs) Handles checkRemove.Click
			If ListObjects Is Nothing OrElse Not(TypeOf ListObjects(0) Is Products) Then
				Return
			End If
			Dim view As ColumnView = TryCast(gclODataV3ServiceMain.FocusedView, ColumnView)
			If view.Columns.Count < 1 OrElse view.FocusedRowHandle < 0 Then
				Return
			End If
			Dim productId As Integer = CInt(Fix(view.GetRowCellValue(view.FocusedRowHandle, view.Columns("ProductID"))))
			Dim actionUri As New Uri(String.Format("{1}({0})/CanRemove", productId, context.Products.RequestUri.AbsoluteUri))
			Dim result As Boolean = context.Execute(Of Boolean)(actionUri, "POST", True).Single()
			If result Then
				MessageBox.Show("Can remove")
			Else
				MessageBox.Show("Can't remove")
			End If
		End Sub
	End Class

End Namespace
