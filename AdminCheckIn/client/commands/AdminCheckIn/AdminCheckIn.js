Type.registerNamespace("Extensions");

Extensions.AdminCheckIn = function Extensions$AdminCheckIn()
{
    Type.enableInterface(this, "AdminCheckIn.Interface");
	this.addInterface("Tridion.Cme.Command", ["AdminCheckIn"]);
};

Extensions.AdminCheckIn.prototype.isAvailable = function AdminCheckIn$isAvailable(selection, pipeline)
{
	var adminCheckinConfig = $config.Editors.AdminCheckIn.configuration;
	// Have to replace the namespace to fix weird issue in IE where the getElementsByTagName method returns empty when the namespace is there
	adminCheckinConfig = adminCheckinConfig.replace("xmlns=\"http://www.sdltridion.com/2009/GUI/Configuration/Merge\"", ""); 
	var xmlDoc = $xml.getNewXmlDocument(adminCheckinConfig);
	var groupsList = xmlDoc.getElementsByTagName("group");

	var groups = Tridion.UI.UserSettings.getJsonUserSettings().User.Data.GroupMemberships;
	var enableUserList = false;

	for(var i = 0; i < groupsList.length; i++)
	{
		if (groups == null)
		{
			enableUserList = false;
		}
		else
		{
			if (groups.length == null)
			{
				if (groups.Group["@title"] == groupsList[i].textContent || groups.Group["@title"] == groupsList[i].text)
				{
					enableUserList = true;
				}
			}
			else if (groups.length >= 1)
			{
				for (var j = 0; j < groups.length; j++)
				{
					if (groups[j]["@title"] == groupsList[i].textContent || groups[j]["@title"] == groupsList[i].text)
					{
						enableUserList = true;
					}
				}
			}
		}
	}

	if (selection && selection.getCount() == 1)
	{
		var itemType = selection.getItemType(0);
		return ((itemType == $const.ItemType.COMPONENT || itemType == $const.ItemType.PAGE) && enableUserList)
	}
	
	return false;
};

Extensions.AdminCheckIn.prototype.isEnabled = function AdminCheckIn$isEnabled(selection, pipeline) {
    var items = selection.getItems();
    if (items.length == 1) {        
            return true;
    }
    else {
        return false;
    }
};


Extensions.AdminCheckIn.prototype._execute = function AdminCheckIn$_execute(selection, pipeline) {

    var selectedID = selection.getItem(0);
    $extUtils.getStaticItem(selectedID, function (item) {
        $log.message("AdminCheckIn$_execute: item has been statically loaded: " + item.isStaticLoaded());        
        var itemTitle = item.getStaticTitle();
        
        var selectedID = selection.getItem(0);
	var host = window.location.protocol + "//" + window.location.host;
	var url = host + '/WebUI/Editors/AdminCheckIn/client/pages/checkin.aspx?Uri=' + selectedID;
	var popup = $popup.create(url, "toolbar=no,width=400px,height=200px,resizable=false,scrollbars=false", null);
    	popup.open();

    });    
};