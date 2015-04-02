using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Plenom.Components.Busylight.Sdk;

namespace RecordingLight
{
  public partial class frmMain : Form
  {
    private BusylightLyncController device;
    private bool die;

    public frmMain()
    {
      InitializeComponent();
      device = new BusylightLyncController();
      if (!device.IsDeviceAttached())
      {
        MessageBox.Show("You don't have a Busylight connected, or I can't see it");
      }
    }

    private void Pulse(BusylightColor color, int delayInMiliseconds = 50)
    {
      if (delayInMiliseconds < 30) delayInMiliseconds = 30;

      die = false;
      int holdblue = color.BlueRgbValue;
      int holdred = color.RedRgbValue;
      int holdgreen = color.GreenRgbValue;

      while (true)
      {
        color.BlueRgbValue -= 10;
        color.RedRgbValue -= 10;
        color.GreenRgbValue -= 10;

        if (color.BlueRgbValue < 0)
          color.BlueRgbValue = holdblue;
        if (color.RedRgbValue < 0)
          color.RedRgbValue = holdred;
        if (color.GreenRgbValue < 0)
          color.GreenRgbValue = holdgreen;

        device.Light(color);
        System.Threading.Thread.Sleep(delayInMiliseconds);
        Application.DoEvents();
        if (die) break;
      }
      device.Light(BusylightColor.Yellow);
    }

    private void Pulse(List<BusylightColor> colors, int delayInMiliseconds = 50)
    {
      if (delayInMiliseconds < 30) delayInMiliseconds = 30;

      die = false;
      while (true)
      {
        foreach (BusylightColor color in colors)
        {
          device.Light(color);
          System.Threading.Thread.Sleep(delayInMiliseconds);

          Application.DoEvents();
          if (die) break;
        }
        if (die) break;
      }
      device.Light(BusylightColor.Yellow);
    }

    private void btnAlert_Click(object sender, EventArgs e)
    {
      //Pulse(BusylightColor.Green);
      //Pulse(new BusylightColor{RedRgbValue = 20, GreenRgbValue = 100, BlueRgbValue = 30});
      List<BusylightColor> colors = new List<BusylightColor> { BusylightColor.Red, BusylightColor.Off, BusylightColor.Blue, BusylightColor.Off };
      Pulse(colors, 500);
    }

    private void button1_Click(object sender, EventArgs e)
    {
      //device.Light(BusylightColor.Off);
      //device.StopKeepAliveSignaling();
      die = true;
    }
  }
}
