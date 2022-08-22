<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128586353/15.2.4%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E4389)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [XpoOdataService.svc.cs](./CS/ODataService/XpoOdataService.svc.cs) (VB: [XpoOdataService.svc.vb](./VB/ODataService/XpoOdataService.svc.vb))
* [ClientForm.cs](./CS/ODataV3Example/ClientForm.cs) (VB: [ClientForm.vb](./VB/ODataV3Example/ClientForm.vb))
<!-- default file list end -->
# OBSOLETE - How to: Use the XPO OData v3 Service

>**NOTE**: Starting with v22.1.4, you can no longer select the *DevExpress ORM OData v3 Service* project template in Visual Studio. This project template itself and WCF Data Services 5.0 (OData v3) are outdated. OData v4 is the most recent standard that provides much more features than OData v3 ([learn more](https://www.odata.org/blog/odata-v4-0-approved-as-a-new-oasis-standard/)).
Use the **[Web API Service](https://docs.devexpress.com/eXpressAppFramework/403394/backend-web-api-service)** with integrated authorization & CRUD operations based on [ASP.NET Core OData 8.0](https://github.com/OData/AspNetCoreOData) (OData v4) powered by EF Core and XPO ORM library instead. For more information, see [XPO - The DevExpress ORM OData v3 Service project template has been removed from the Template Gallery](https://supportcenter.devexpress.com/internal/ticket/details/T1097832).

----------------

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
1. Composite keys are not currently supported by XpoDataServiceV3 and it is unlikely that they will be supported in the near future. You may receive the "_The entity type 'XXX' does not have any key properties. Please make sure that one or more key properties are defined for this entity type._" error with XpoDataServiceV3 if you map your persistent class to a table or view with a multi-column key. Since the XAF Mobile UI uses XPO OData Service, this will not work there as well.  In general, we do not recommend using composite keys as they impose many limitations on the default functionality. Refer to the [How to create a persistent object for a database table with a compound key](https://www.devexpress.com/Support/Center/p/A2615) article for more details.  
2. A WCF Data Service supports custom service methods (Service Operations and Actions) as standard features. Please refer to the corresponding MSDN articles to learn how to implement and use these features in your service: [Service Operations](http://msdn.microsoft.com/en-us/library/cc668788%28v=vs.103%29), [Service Actions](http://msdn.microsoft.com/en-us/library/hh859851%28v=vs.103%29).
3. Custom non-persistent types cannot used by the XpoDataServiceV3, which is designed to operate persistent types only. So, for this task, I suggest you continue using a regular non-XPO service. Refer to the [Q577141](https://www.devexpress.com/Support/Center/p/Q577141) article for possible solutions.  
4. XpoDataServiceV3 provides partial support for annotations. The static _XpoDataServiceV3.CreateAnnotationsBuilder_ method expects a delegate to obtain _XpoContext_ and returns a delegate that adds string fields sizes and read-only flags descriptions to the annotation. You can put this delegate to the service configuration to get these annotations.
   ```csharp
   public class XpoTestService : XpoDataServiceV3 {
    public static void InitializeService(DataServiceConfiguration config) {
      ...
      config.AnnotationsBuilder = CreateAnnotationsBuilder(MyGetXpoContextDelegate);
    }
   }
   ```
   If you need to include additional information in annotations, wrap this delegate with your own extended delegate.  
5. Properties of the System.Drawing.Image and byte[] types are mapped to the _DataServiceStreamLink_ type in a service reference (Edm.Stream). To get data from this DataServiceStreamLink, use the DataServiceContext.GetReadStream method. Then load an image from the returned Stream. Here is an example from the article's ClientForm.cs file:
   ```csharp
   Category category = context.Categories.Where(i => i.CategoryID == categoryID).Single();
   DataServiceStreamResponse resp = context.GetReadStream(category, "StreamPicture", new DataServiceRequestArgs());
   pictureBox.Image = Image.FromStream(resp.Stream);
   ```
To use byte arrays instead of DataServiceStreamLink in your service, override the _XpoContext.ShowLargePropertyAsNamedStream_ method and returning False in it. Refer to the [Working with Binary Data (WCF Data Services)](https://docs.microsoft.com/en-us/dotnet/framework/data/wcf/working-with-binary-data-wcf-data-services) article for more details.


