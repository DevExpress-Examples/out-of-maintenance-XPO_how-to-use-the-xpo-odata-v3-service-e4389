Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Xpo
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl

Namespace Demo.Module
	<DefaultClassOptions> _
	Public Class Document
		Inherits BaseObject
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Private _name As String
		Public Property Name() As String
			Get
				Return _name
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Name", _name, value)
			End Set
		End Property
		<Association("Document-Articles"), Aggregated> _
		Public ReadOnly Property Articles() As XPCollection(Of Article)
			Get
				Return GetCollection(Of Article)("Articles")
			End Get
		End Property
	End Class
End Namespace
