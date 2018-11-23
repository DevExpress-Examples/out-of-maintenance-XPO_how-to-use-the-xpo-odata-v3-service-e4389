Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Data.Services
Imports Northwind
Imports DevExpress.Xpo
Imports DevExpress.Xpo.DB
Imports System.ServiceModel
Imports System.IO
Imports System.Web
Imports System.Data.Services.Common
Imports System.Linq.Expressions
Imports System.ServiceModel.Web
Imports System.Linq
Imports System.Web.Security

Namespace XpoODataExample
	Public Class XpoODataContext
		Inherits XpoContext
		Public CategoriesFiltering As Boolean = False
		Public Sub New(ByVal s1 As String, ByVal s2 As String, ByVal dataLayer As IDataLayer)
			MyBase.New(s1, s2, dataLayer)
		End Sub
		<Action> _
		Public Function CanRemove(ByVal product As Products) As Boolean
			Return Not(New XPQuery(Of Suppliers)(product.Session)).Any(Function(s) s.Products.Contains(product))
		End Function
	End Class

	<ServiceBehavior(InstanceContextMode := InstanceContextMode.PerCall)> _
	Public Class XpoODataExampleService
		Inherits XpoDataServiceV3
		Private Shared ReadOnly context As New XpoODataContext("XpoExampleContext", "Northwind", CreateDataLayer())

		Public Sub New()
			MyBase.New(context, context)
		End Sub

		Private Shared Function CreateDataLayer() As IDataLayer
			Dim dict As DevExpress.Xpo.Metadata.XPDictionary = New DevExpress.Xpo.Metadata.ReflectionDictionary()
			' Initialize the XPO dictionary. 
			dict.GetDataStoreSchema(GetType(Orders).Assembly)
			Dim store As New InMemoryDataStore(AutoCreateOption.SchemaOnly)
			Dim DBFileName As String = DevExpress.Utils.FilesHelper.FindingFileName(HttpRuntime.AppDomainAppPath, "App_Data\nwind.xml")
			If DBFileName <> "" Then
				store.ReadXml(DBFileName)
			End If
			Dim dataLayer As IDataLayer = New ThreadSafeDataLayer(dict, store)
			XpoDefault.DataLayer = dataLayer
			XpoDefault.Session = Nothing
			Return dataLayer
		End Function
		Protected Overrides Sub OnStartProcessingRequest(ByVal args As ProcessRequestArgs)
			MyBase.OnStartProcessingRequest(args)
		End Sub
		Public Shared Sub InitializeService(ByVal config As DataServiceConfiguration)
			config.SetEntitySetAccessRule("*", EntitySetRights.All)
			config.DataServiceBehavior.AcceptAnyAllRequests = True
			config.UseVerboseErrors = True
			config.SetServiceOperationAccessRule("*", ServiceOperationRights.All)
			config.SetServiceActionAccessRule("*", ServiceActionRights.Invoke)
			config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3
			config.DataServiceBehavior.AcceptProjectionRequests = True
			config.DataServiceBehavior.AcceptCountRequests = True
			config.AnnotationsBuilder = CreateAnnotationsBuilder(Function() context)
			config.DataServiceBehavior.AcceptReplaceFunctionInQuery = True
			config.DataServiceBehavior.AcceptSpatialLiteralsInQuery = True
			config.DisableValidationOnMetadataWrite = True
		End Sub

		<QueryInterceptor("Categories")> _
		Public Function OnQueryActors() As Expression(Of Func(Of Categories, Boolean))
			If context.CategoriesFiltering Then
				Return Function(o) o.CategoryName.Length > 12
			Else
				Return Function(o) True
			End If
		End Function

		<ChangeInterceptor("Products")> _
		Public Sub OnChangeProducts(ByVal product As Products, ByVal operations As UpdateOperations)
			If Equals(operations, UpdateOperations.Change) Then
				Dim productInBase As Products = GetEntityUpdated(Of Products)(product)
				If productInBase.ProductName <> product.ProductName Then
					Throw New DataServiceException(400, String.Format("A {0} cannot be modified.", product.ToString()))
				End If
			ElseIf Equals(operations, UpdateOperations.Delete) Then
				Throw New DataServiceException(400, "Products cannot be deleted.")
			End If
		End Sub

		Public Overrides Function Authenticate(ByVal args As ProcessRequestArgs) As Object
			Dim oid As Integer
			Try
				Dim authCookie As String = args.OperationContext.RequestHeaders("userCookie")
				Dim ticket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(authCookie)
				oid = Integer.Parse(ticket.UserData)
			Catch
				Return Nothing
			End Try
			Using uow As New UnitOfWork(Context.ObjectLayer)
				Dim cont As Employees = uow.GetObjectByKey(Of Employees)(oid)
				Return If(cont IsNot Nothing, "ok", ":(")
			End Using
		End Function

		Public Overrides Function GetQueryInterceptor(ByVal entityType As Type, ByVal token As Object) As LambdaExpression
			If token Is Nothing Then
				Throw New DataServiceException(401, "Unauthorized access.")
			End If
			If CStr(token) = "ok" Then
				Return CType(Function(o) True, Expression(Of Func(Of Object, Boolean)))
			Else
				Return CType(Function(o) False, Expression(Of Func(Of Object, Boolean)))
			End If
		End Function

		<WebGet> _
		Public Function GetAuthorizationCookie(ByVal name As String, ByVal password As String) As String
			Using uow As New UnitOfWork(Context.ObjectLayer)
				Dim contacts As List(Of Employees) = New XPQuery(Of Employees)(uow).Where(Function(i) i.LastName = name).ToList()
				Dim contact As Employees = contacts.SingleOrDefault()

				If contact Is Nothing Then
					Return String.Empty
				End If
				Dim ticket As New FormsAuthenticationTicket(1, contact.LastName, DateTime.Now, DateTime.Now.AddMinutes(30), True, contact.EmployeeID.ToString())

				Dim encryptedTicket As String = FormsAuthentication.Encrypt(ticket)
				Return encryptedTicket
			End Using
		End Function

		<WebGet> _
		Public Sub SetCategoriesFiltering(ByVal state As Boolean)
			context.CategoriesFiltering = state
		End Sub
	End Class
End Namespace
