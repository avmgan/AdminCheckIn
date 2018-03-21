Steps to install:

1. Copy folder AdminCheckIn and drop in "<Tridion Install>\web\WebUI\Editors" folder

2. Go to your WebRoot\bin folder and copy the dll's within that folder and drop in the "<Tridion Install>\web\WebUI\Editors\AdminCheckIn\bin\" folder.

3. Under the Tridion 2011 Website in IIS -> Navigate to WebUI\Editors directory
	3a. Right Click and select "Add new application".  Name the Application "AdminCheckIn" and set the physical path to the path you've just dropped the folder in step 1.

4. Navigate to "<Tridion Install>\web\WebUI\WebRoot\Configuration" and open System.config
	4a. Under the <Editors> element, add the element for your new WebUI:
		<editor name="AdminCheckIn">
              		<installpath>C:\Tridion\web\WebUI\Editors\AdminCheckIn\</installpath>
              		<configuration>C:\Tridion\web\WebUI\Editors\AdminCheckIn\Config\admincheckincm.config</configuration>
              		<vdir>AdminCheckIn</vdir><!-- Must match IIS Virtual Dir name -->
    		</editor>
	4b. Make sure you put the correct installPath and configuration path and save and close.

5. Go to "<Tridion Install>\web\WebUI\Editors\AdminCheckIn\Config\" and open admincheckincm.config

6. In the element:
	<customconfiguration>
	    <clientconfiguration>
		    <checkin>
			<admin>
				<checkInAdminUsername>AdminUser</checkInAdminUsername>
				<checkInAdminPassword>AdminPassword</checkInAdminPassword>
			</admin>
			<users>
				<group>group1</group>
				<group>group2</group>
			</users>
		    </checkin>
	    </clientconfiguration>
	</customconfiguration>
	
	6a. Fill in the checkInAdminUsername with the username of the Administrator you wish to impersonate
	6b. Fill in the checkInAdminPassword wiht the password of the Administrator you wish to impersonate
	6c. Fill in the group for the User Group you wish to enable this feature on. (You may specify more than one group)
	6d. Save and Close and restart IIS

7. Log in to Tridion as the user whose group is in the list of user groups you specified in the admincheckincm.config in step 5.  When you select a Component or Page, right click to pull up the Context Menu.  There you will see "Admin CheckIn" feature added.
