# SunnyPoint-Azure

Microsoft Azure Subscription : You must have one Microsoft Azure subscription account to deploy all follow Azure services. And, it is good practices to allocate and group all follows services into one Azure Resource Group.

Step 1: Create Azure Resource Group: UBIDriving
Step 2: Azure SQL
     - Create Azure SQL services under UBIDriving group. You need to keep DBConnectionString at hand, which will be configuration for couple other service.
     - Run GitHub:/spDatabase/*.sql by ordering to create Database schema.
     
Step 3: Create IoTHub. Just give a name, and use all default value and free tier. You need to keep IoTHub Host name and Connection String.
