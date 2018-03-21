using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TridionAdminCheckIn.CoreService;

public partial class SamplePage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Text = "Checking in item";
	string value = Request.QueryString["Uri"];
	value = value.Replace("%3A", ":");
	TridionCheckIn.CoreService.TridionCheckIn tridionCheckIn = new TridionCheckIn.CoreService.TridionCheckIn();
	tridionCheckIn.CheckInItem(value, Label1);
	tridionCheckIn.Dispose();
    }
}