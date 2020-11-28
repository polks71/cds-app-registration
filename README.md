# Common Data Service (DataVerse) and Azure App Registration
This repository shows a very simple example connecting to the [Common Data Service](https://docs.microsoft.com/en-us/powerapps/maker/common-data-service/data-platform-intro) using an [Azure Active Directy App Registration](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app). This is important with the announced deprecation of the [Office 365 authentication to the CDS](https://docs.microsoft.com/en-us/power-platform/important-changes-coming#deprecation-of-office365-authentication-type-and-organizationserviceproxy-class-for-connecting-to-dataverse). In this example I will show the following steps: 
- [How to create an Azure AD App Registration](#create-an-azure-ad-app-registration)
- [Add the App Registration to Dynamics 365 as an App user](#add-the-app-registration-to-dynamics-365-as-an-app-user)
- [Use authentication in a simple .Net console application](#use-authentication-in-a-simple-.net-console-application)

# Create an Azure AD App Registration
The App Registration needs to be in the same Azure Tenant as the Dynamics 365 environment, when you add the user to Dynamics 365 there is not an option to add a Tenant Id. So, when we create the App Registration we do not need to worrry about authenticating account outside of the current tenant, at least I have not run into a scenario where I need to when doing this for D365.


- Navigate to the [Azure Portal](https://portal.azure.com). Login with credentials that can register an application.
- Expand the menu in the top left and navigate to Azure Active Directory

![](images\navigate-to-azure-ad.png?raw=true)

- Then navigate to **App Registrations**
- Then click on **New registration**

![](images\navigate-to-app-registrations.png?raw=true)

- Fill in the application details
    - Enter a meaningful name
    - Select Accounts in this organization.
    - Leave the Redirect URI blank
- Click on Register

![](images\create-app-registration.png?raw=true)

- Click on **Certificates & Secrets**
- Click on "+ New Client Secret"

![](images\add-client-secret.png?raw=true)

- Enter a meaningful name
- Select the expiration of the secret
- Click on Add

![](images\add-new-secret-one-year.png?raw=true)

- Save the "Value" in a place you will not lose it. This is your one chance to view the full value.

![](images\client-secret-details.png?raw=true)

- Click on **Overview**

![](images\app-registration-overview.png?raw=true)


- For the next steps we need to have the following values
    - Client Secret Value (from previous step)
    - The App ID
    - Tenant Id

# Add the App Registration to Dynamics 365 as an App user
For this step of the process we navigate to the Dynamics 365 application for adding the App Registration as a System User. As of this writing the new admin interface does not have the functionality so you will need to navigate to the classic web admin interface. 

- https://make.powerapps.com
- Select the environment you need to add the user to

![](images\make-select-environment.png?raw=true)

- Click on the gear in the upper right corner
- Click on Advanced Settings

![](images\make-select-advanced-settings.png?raw=true)

- Click on the down caret next to **Settings**
- Click **Security** under **System**
- Then click on **Users**

![](images\web-navigate-security.png?raw=true)

![](images\web-navigate-security-users.png?raw=true)

- Expand the list of Views
- Select **Application Users**

![](images\web-navigate-security-users-application.png?raw=true)

- Then click on the **+ New** button

![](images\web-navigate-security-users-new.png?raw=true)

- Make sure you select the **Application User** form

![](images\web-navigate-security-new-user-form-select.png?raw=true)

- Enter the Azure AD Application Registration Id (AppId from above steps)
- Enter a name for the Application (First and Last Name)
- Enter an email
    - For internal use any email address will suffice.
    - For an ISV, for example, you could put your support email address here
- Then click Save

![](images\web-new-application-user-create-details.png?raw=true)

- Then assign the appropriate roles to the user using the **Manage Roles** at the top of the screen

# Use Authentication in a Simple .Net Console Application


