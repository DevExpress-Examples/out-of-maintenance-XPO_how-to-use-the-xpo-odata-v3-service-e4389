<!-- default file list -->
*Files to look at*:

* [XpoOdataService.svc.cs](./CS/ODataService/XpoOdataService.svc.cs) (VB: [XpoOdataService.svc.vb](./VB/ODataService/XpoOdataService.svc.vb))
* [ClientForm.cs](./CS/ODataV3Example/ClientForm.cs) (VB: [ClientForm.vb](./VB/ODataV3Example/ClientForm.vb))
<!-- default file list end -->
# How to: Use the XPO OData V3 Service

To start the service, run the **ODataService** project. To start the client application, run the **ODataV3Example** project.  

## See Also
- [How to Implement OData v4 Service with XPO (.NET Core 3.1)](https://github.com/DevExpress-Examples/XPO_how-to-implement-odata4-service-with-xpo-netcore)
- [How to Implement OData v4 Service with XPO (.NET Framework)](https://github.com/DevExpress-Examples/XPO_how-to-implement-odata4-service-with-xpo)
- [DevExtreme + OData v4 Service with XPO + XAF's User Authentication and Group Authorization API (Powered by XPO)](https://www.devexpress.com/go/XAF_Security_NonXAF_Series_2.aspx)
- [How to implement CRUD operations using XtraGrid and OData](https://github.com/DevExpress-Examples/how-to-implement-crud-operations-using-xtragrid-and-odata-e4070)  
- [How to implement CRUD operations using XtraGrid and WCF Server Mode](https://github.com/DevExpress-Examples/how-to-implement-crud-operations-using-xtragrid-and-wcf-server-mode-e4365)  
- [GridControl - How to use the WcfServerModeSource component as a data source](https://www.devexpress.com/Support/Center/p/K18557)  
- [How to process authenticated requests on the OData service](https://github.com/DevExpress-Examples/how-to-process-authenticated-requests-on-the-odata-service-e4403)  
- [How to send authenticated requests to the OData service](https://github.com/DevExpress-Examples/how-to-send-authenticated-requests-to-the-odata-service-e4460)  

## Important Notes
1. An OData service created using the [OData Service Wizard](https://documentation.devexpress.com/CoreLibraries/CustomDocument14812.aspx) is a **standard** [WCF Data Service 5+](https://msdn.microsoft.com/library/hh487257(v=vs.103).aspx) for OData v3\. We don't provide documentation on how to consume this service because this task has no XPO-related specifics. Use standard solutions recommended for [OData](http://www.odata.org/) services in public community resources like MSDN and StackOverFlow. For instance, you may find these resources helpful:  
&nbsp;&nbsp;&nbsp;&nbsp;[WCF Data Services Client Library](https://msdn.microsoft.com/en-us/library/cc668772.aspx)  
&nbsp;&nbsp;&nbsp;&nbsp;[Updating the Data Service (WCF Data Services)](https://docs.microsoft.com/en-us/dotnet/framework/data/wcf/updating-the-data-service-wcf-data-services) and [How to: Add, Modify, and Delete Entities (WCF Data Services)](https://msdn.microsoft.com/en-us/library/dd756368(v=vs.110).aspx)   
&nbsp;&nbsp;&nbsp;&nbsp;[DataServiceContext.SaveChanges](https://msdn.microsoft.com/en-us/library/cc646716(v=vs.110).aspx) and [DataServiceContext](https://msdn.microsoft.com/en-us/library/system.data.services.client.dataservicecontext.aspx)  
2. Composite keys are not currently supported by XpoDataServiceV3 and it is unlikely that they will be supported in the near future. You may receive the "_The entity type 'XXX' does not have any key properties. Please make sure that one or more key properties are defined for this entity type._" error with [OData Service](https://documentation.devexpress.com/CoreLibraries/14812/DevExpress-ORM-Tool/Design-Time-Features/OData-Service-Wizard) (XpoDataServiceV3) if you map your persistent class to a table or view with a multi-column key. Since the XAF Mobile UI uses XPO OData Service, this will not work there as well.  In general, we do not recommend using composite keys as they impose many limitations on the default functionality. Refer to the [How to create a persistent object for a database table with a compound key](https://www.devexpress.com/Support/Center/p/A2615) article for more details.  
3. A WCF Data Service supports custom service methods (Service Operations and Actions) as standard features. Please refer to the corresponding MSDN articles to learn how to implement and use these features in your service: [Service Operations](http://msdn.microsoft.com/en-us/library/cc668788%28v=vs.103%29), [Service Actions](http://msdn.microsoft.com/en-us/library/hh859851%28v=vs.103%29).  
4. With the 17.1 release, [non-persistent](https://help.devexpress.com/#CoreLibraries/CustomDocument2056) property values can be loaded using the [XPO OData Service](https://help.devexpress.com/#CoreLibraries/CustomDocument14812). To enable this functionality, apply the [Visible](https://help.devexpress.com/#CoreLibraries/clsDevExpressXpoVisibleAttributetopic) attribute to a non-persistent property. Our XPO Odata Service will automatically execute your custom business logic associated with this property and return the resulting value to the client.  
5. Custom non-persistent types cannot used by the XpoDataServiceV3, which is designed to operate persistent types only. So, for this task, I suggest you continue using a regular non-XPO service. Refer to the [Q577141](https://www.devexpress.com/Support/Center/p/Q577141) article for possible solutions.  
6. XpoDataServiceV3 provides partial support for annotations. The static _XpoDataServiceV3.CreateAnnotationsBuilder_ method expects a delegate to obtain _XpoContext_ and returns a delegate that adds string fields sizes and read-only flags descriptions to the annotation. You can put this delegate to the service configuration to get these annotations.
   ```csharp
   public class XpoTestService : XpoDataServiceV3 {
    public static void InitializeService(DataServiceConfiguration config) {
      ...
      config.AnnotationsBuilder = CreateAnnotationsBuilder(MyGetXpoContextDelegate);
    }
   }
   ```
   If you need to include additional information in annotations, wrap this delegate with your own extended delegate.  
7. Properties of the System.Drawing.Image and byte[] types are mapped to the _DataServiceStreamLink_ type in a service reference (Edm.Stream). To get data from this DataServiceStreamLink, use the DataServiceContext.GetReadStream method. Then load an image from the returned Stream. Here is an example from the article's ClientForm.cs file:
   ```csharp
       DataServiceStreamResponse resp = context.GetReadStream(context.Categories.Where(i => i.CategoryID == categoryID).Single(), "StreamPicture", new DataServiceRequestArgs());
       pictureBox.Image = Image.FromStream(resp.Stream);
   ```
To use byte arrays instead of DataServiceStreamLink in your service, override the _XpoContext.ShowLargePropertyAsNamedStream_ method and returning False in it. Refer to the [Working with Binary Data (WCF Data Services)](https://docs.microsoft.com/en-us/dotnet/framework/data/wcf/working-with-binary-data-wcf-data-services) article for more details.


