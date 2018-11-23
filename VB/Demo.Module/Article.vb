Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Xpo
Imports DevExpress.Persistent.BaseImpl

Namespace Demo.Module
	Public Class Article
		Inherits BaseObject
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Private _subject As String
		Public Property Subject() As String
			Get
				Return _subject
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Subject", _subject, value)
			End Set
		End Property
		Private _description As String
		<Size(SizeAttribute.Unlimited)> _
		Public Property Description() As String
			Get
				Return _description
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Description", _description, value)
			End Set
		End Property
		Private _body As String
		<Size(SizeAttribute.Unlimited)> _
		Public Property Body() As String
			Get
				Return _body
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Body", _body, value)
			End Set
		End Property
		Private _document As Document
		<Association("Document-Articles")> _
		Public Property Document() As Document
			Get
				Return _document
			End Get
			Set(ByVal value As Document)
				SetPropertyValue("Document", _document, value)
			End Set
		End Property
		Private _createdOn As DateTime
		Public Property CreatedOn() As DateTime
			Get
				Return _createdOn
			End Get
			Set(ByVal value As DateTime)
				SetPropertyValue("CreatedOn", _createdOn, value)
			End Set
		End Property
	End Class
End Namespace