Steps to install:

1. Copy folder AdminCheckIn and drop in "&lt;Tridion Install&gt;\web\WebUI\Editors" folder

2. Go to your WebRoot\bin folder and copy the dll's within that folder and drop in the "&lt;Tridion Install&gt;\web\WebUI\Editors\AdminCheckIn\bin\" folder.

3. Under the Tridion 2011 Website in IIS -&gt; Navigate to WebUI\Editors directory
	3a. Right Click and select "Add new application".  Name the Application "AdminCheckIn" and set the physical path to the path you've just dropped the folder in step 1.

4. Navigate to "&lt;Tridion Install&gt;\web\WebUI\WebRoot\Configuration" and open System.config
	4a. Under the &lt;Editors&gt; element, add the element for your new WebUI:
		&lt;editor name="AdminCheckIn"&gt;
              		&lt;installpath&gt;C:\Tridion\web\WebUI\Editors\AdminCheckIn\&lt;/installpath&gt;
              		&lt;configuration&gt;C:\Tridion\web\WebUI\Editors\AdminCheckIn\Config\admincheckincm.config&lt;/configuration&gt;
              		&lt;vdir&gt;AdminCheckIn&lt;/vdir&gt;&lt;!-- Must match IIS Virtual Dir name --&gt;
    		&lt;/editor&gt;
	4b. Make sure you put the correct installPath and configuration path and save and close.

5. Go to "&lt;Tridion Install&gt;\web\WebUI\Editors\AdminCheckIn\Config\" and open admincheckincm.config

6. In the element:
	&lt;customconfiguration&gt;
	    &lt;clientconfiguration&gt;
		    &lt;checkin&gt;
			&lt;admin&gt;
				&lt;checkInAdminUsername&gt;AdminUser&lt;/checkInAdminUsername&gt;
				&lt;checkInAdminPassword&gt;AdminPassword&lt;/checkInAdminPassword&gt;
			&lt;/admin&gt;
			&lt;users&gt;
				&lt;group&gt;group1&lt;/group&gt;
				&lt;group&gt;group2&lt;/group&gt;
			&lt;/users&gt;
		    &lt;/checkin&gt;
	    &lt;/clientconfiguration&gt;
	&lt;/customconfiguration&gt;
	
	6a. Fill in the checkInAdminUsername with the username of the Administrator you wish to impersonate
	6b. Fill in the checkInAdminPassword wiht the password of the Administrator you wish to impersonate
	6c. Fill in the group for the User Group you wish to enable this feature on. (You may specify more than one group)
	6d. Save and Close and restart IIS

7. Log in to Tridion as the user whose group is in the list of user groups you specified in the admincheckincm.config in step 5.  When you select a Component or Page, right click to pull up the Context Menu.  There you will see "Admin CheckIn" feature added.
