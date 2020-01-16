This package works on Umbraco 8.1.0 or higher.

# Installation
The recommended way to install the package is via nuget.
If you install the package via nuget, nuget will handle all the configuration during installation.

The package can be installed as an Umbraco Package as well, we recommend to install the package this way only if fore some reason nuget is not available.
The package has a dependency for another nuget package, in the umbraco package the dll of this dependency is just copied in.
This is not a clean way of installing packages and can be a mess if you would want to update this dependency without updating the package.

# How to use
First, if you want to enable 2fa on your Umbraco site, you'll need to add the special 2fa section to the user roles that you want to be able to control the 2fa settings of the site.
In this section you can remove configured 2fa from other users and you can set the permission about 2fa for other user roles.
A user is only allowed to add 2fa to his account if he has a user role that has a form of 2fa allowed in the permission screen.
If a user has a role that has 2fa required, he will be prompted to add a 2fa device during his next login and cannot continue without enabling 2fa.

In the register screen a user can add 2fa to his account.
In the manage 2fa screen a user can manage his 2fa devices and remove devices that he doesn't use anymore.
If a user removes all his devices, 2fa will be disabled again for this user until a new device is added.

We recommend always using the mobile app as it is the safest protocol for 2fa and is the hardest to intercept.

# Configuration
This package has some settings about the package that can be configured.
These options can be found in the config file located at `App_Plugins/TwoFactor/TwoFactor.config`.

- appName: You should enter the name of the website here. This name is what people will see on their 2fa app if the scan a QR code.

- pageSize: This is a visual only setting.
It sets the amount of users that appear on a single page in the 2fa section where you can reset 2fa of other users.

- timeoutTime: This is the amount of minutes a user has to enter their 2fa code during login.
It is recommended to keep this at 5 as it is a good balance between security and ease of use

- browserRememberTime: This setting is used to enable cookie authentication for the second step.
If you change this number to anything above 0 it will enable cookie authentication.
This means that users will be allowed to skip 2fa for the amount of minutes configured in this setting.
This setting defaults to 0 as it is the safest to always use 2fa but can be enabled for convience.
If you do enable it, it is recommended to keep the cookie time at a maximum of a week(10.080 minutes).

- mailSettings: There are multiple mail settings, these settings are used when using mail authentication for 2fa.
**If you want to use mail authentication you will still need to configure smtp settings in the web.config.**
The mail settings are pretty self explanatory.
"fromAddress" is the mail address where the 2fa mail is sent from, "subject" is the subject of the 2fa mail and "message" is the content of the mail.
The "message" configuration should always contain {0} This wil be replace with the 2fa code in the actual mail.

# Mail Authentication
If you want to be able to use mail authentication you will need to configure an smtp server yourself.
You can do this the same way as sending any other mail via the web.config.
This package uses the same settings from the web.config.

# Changing the backoffice URL
If you changed the backoffice URL of your site you will need to change it in this package as well otherwise the package will not function anymore.
Because of a refresh issue during login, the package needs to know the backoffice URL.
If you change the URL you will need to change it in a javascript constants file as well.
This file is located at `App_Plugins/TwoFactor/Constants.js`.
In this file you will need to change `AdminURL` to the new backoffice url that you have configured.

# Contribute
If you want to contribute to the package you can do so by making an issue on the github, or by making a pull request from your fork.
To use the source locally you should do the following.
Create a new solution in visual studio and create a new Umbraco 8 site.
Add the the TwoFactor folder from the testSite folder to the App_Plugins folder of your fresh Umbraco install.
Than add the TwoFactorAuth project to your solution and add a reference to it in your Test Site.
When you want to commit changes to the package you should only commit the folders that are in the github right now to keep the repository clean.
