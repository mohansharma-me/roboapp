using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Robo
{
    public partial class UserSettings : Form
    {
        public UserSettings()
        {
            InitializeComponent();
        }

        private void UserSettings_Load(object sender, EventArgs e)
        {
            txtstartflusing.Text = UserSetting.StartFlusingMessage;
            txtStatusFormat.Text = UserSetting.StatusMessageFormat;
            txtSettingFormat.Text = UserSetting.SettingMessageFormat;
            txtsfbw.Text = UserSetting.SandFilterBackWashMessage;
            txtcfbw.Text = UserSetting.CarbonFilterBackWashMessage;
            txtmachinestart.Text = UserSetting.MachineStartMessage;
            txtmachinestop.Text = UserSetting.MachineStopMessage;
            txtreadsetting.Text = UserSetting.ReadSettingMessage;
            txtreadstatus.Text = UserSetting.ReadStatusMessage;
            txtsofbw.Text = UserSetting.SoftnerFilterBackWashMessage;
            txtwriteseparator.Text = UserSetting.WriteSettingSeparator;
            txtwritesetting.Text = UserSetting.WriteSettingMessage;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            UserSetting.StatusMessageFormat = txtStatusFormat.Text;
            UserSetting.SettingMessageFormat = txtSettingFormat.Text;
            UserSetting.SandFilterBackWashMessage = txtsfbw.Text;
            UserSetting.CarbonFilterBackWashMessage = txtcfbw.Text;
            UserSetting.MachineStartMessage = txtmachinestart.Text;
            UserSetting.MachineStopMessage = txtmachinestop.Text;
            UserSetting.ReadSettingMessage = txtreadsetting.Text;
            UserSetting.ReadStatusMessage = txtreadstatus.Text;
            UserSetting.SoftnerFilterBackWashMessage = txtsofbw.Text;
            UserSetting.WriteSettingSeparator = txtwriteseparator.Text;
            UserSetting.WriteSettingMessage = txtwritesetting.Text;
            UserSetting.StartFlusingMessage = txtstartflusing.Text;
            Close();
        }
    }
}
