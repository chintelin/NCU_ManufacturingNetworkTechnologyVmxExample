using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Composition;
using Itri.Vmx.Host;
using Itri.Vmx.Daq;


namespace MySensingApp
{
    [Export(typeof(IVmxApp))]
    public partial class Form1 : Form, IVmxApp
    {
        #region members
        DaqAdaptor soundCard = null;
        DaqBuffer buffer = null;
        double[] data = null;
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        public string AppName
        {
            get { return "MicSignal"; }
        }

        public Image Image
        {
            get { return Properties.Resources.abc; }
        }

        public bool Initialize(IVmxHost host)
        {
            if (soundCard == null)
                soundCard = host.DaqAdapters[0];

            buffer = new DaqBuffer(soundCard, soundCard.Settings.SamplingRate * 1);
            //soundCard.DataAcquired += new EventHandler<TimeSeriesData>(soundCard_DataAcquired);

            soundCard.Start();
            timer1.Start();
            return true;
        }

        void soundCard_DataAcquired(object s, TimeSeriesData data)
        {
            double[] samples = data.GetSamples(0);
            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            if (data == null)
                data = buffer.CreateContainerForBufferData();

            buffer.GetAllData(0, data);
            chart1.Series[0].Points.DataBindY(data);

            timer1.Start();
        }
    }
}
