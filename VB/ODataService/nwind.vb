Imports DevExpress.Xpo
Imports DevExpress.Xpo.Metadata
Namespace Northwind
    <OptimisticLocking(False)>
    Public Class Shippers
        Inherits XPBaseObject

        Public CompanyName As String
        Public Phone As String
        <Key>
        Public ShipperID As Integer
        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub
    End Class

    <OptimisticLocking(False)>
    Public Class Suppliers
        Inherits XPBaseObject

        Public Address As String
        Public City As String
        Public CompanyName As String
        Public ContactName As String
        Public ContactTitle As String
        Public Country As String
        Public Fax As String
        Public HomePage As String
        <Size(14)>
        Public Phone As String
        Public PostalCode As String
        Public Region As String
        <Association("Product_Supplier", GetType(Products))>
        Public ReadOnly Property Products() As XPCollection(Of Products)
            Get
                Return GetCollection(Of Products)("Products")
            End Get
        End Property
        <Key>
        Public SupplierID As Integer
        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub
    End Class

    <OptimisticLocking(False)>
    Public Class Employees
        Inherits XPBaseObject

        Public Address As String
        Public BirthDate As Date
        Public City As String
        Public Country As String
        <Key>
        Public EmployeeID As Integer
        Public Extension As String
        Public FirstName As String
        Public HireDate As Date
        Public HomePhone As String
        Public LastName As String
        Public Notes As String
        Public Photo() As Byte
        Public PostalCode As String
        Public Region As String
        Public ReportsTo As Integer
        Public Title As String
        Public TitleOfCourtesy As String
        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub
    End Class

    <OptimisticLocking(False)>
    Public Class Orders
        Inherits XPBaseObject

        Public CustomerID As Customers
        Public EmployeeID As Employees
        Public Freight As Decimal
        Public OrderDate As Date
        <Key>
        Public OrderID As Integer
        Public RequiredDate As Date
        Public ShipAddress As String
        Public ShipCity As String
        Public ShipCountry As String
        Public ShipName As String
        Public ShippedDate As Date
        Public ShipPostalCode As String
        Public ShipRegion As String
        Public ShipVia As Shippers
        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub
    End Class

    <OptimisticLocking(False)>
    Public Class Categories
        Inherits XPBaseObject

        <Key>
        Public CategoryID As Integer
        Public CategoryName As String
        Public Description As String
        Private _picture As System.Drawing.Image
        <ValueConverter(GetType(ImageValueConverter)), Stream("StreamPicture", "image/jpeg")>
        Public Property Picture() As System.Drawing.Image
            Get
                Return _picture
            End Get
            Set(ByVal value As System.Drawing.Image)
                SetPropertyValue("Picture", _picture, value)
            End Set
        End Property
        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub
    End Class

    <OptimisticLocking(False)>
    Public Class Products
        Inherits XPBaseObject

        Public CategoryID As Categories
        Public Discontinued As Boolean
        Public EAN13 As String
        <Key>
        Public ProductID As Integer
        Public ProductName As String
        Public QuantityPerUnit As String
        Public ReorderLevel As Short
        <Association("Product_Supplier")>
        Public SupplierID As Suppliers
        Public UnitPrice As Decimal
        Public UnitsInStock As Short
        Public UnitsOnOrder As Short
        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub
        <PersistentAlias("UnitPrice*UnitsInStock")>
        Public ReadOnly Property ExtendedPrice() As Decimal
            Get
                Return Convert.ToDecimal(EvaluateAlias("ExtendedPrice"))
            End Get
        End Property
    End Class

    <OptimisticLocking(False)>
    Public Class Customers
        Inherits XPBaseObject

        Public Address As String
        Public City As String
        Public CompanyName As String
        Public ContactName As String
        Public ContactTitle As String
        Public Country As String
        <Key>
        Public CustomerID As String
        Public Fax As String
        Public Phone As String
        Public PostalCode As String
        Public Region As String
        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub
    End Class
End Namespace
