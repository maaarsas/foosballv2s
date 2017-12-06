using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Emgu.CV.Structure;

namespace foosballv2s.Source.Activities.Adapters
{
    /// <summary>
    /// This class populates a one row in the game list with needed values
    /// </summary>
    class ColorListAdapter : BaseAdapter<Hsv>
    {
        private List<Hsv> colorList;
        private Context cContext;
        public override int Count
        {
            get { return colorList.Count; }
        }

        public ColorListAdapter(Context context, List<Hsv> list)
        {
            colorList = list;
            cContext = context;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Hsv this[int position]
        {
            get { return colorList[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if(row == null)
            {
                row = LayoutInflater.From(cContext).Inflate(Resource.Layout.Color_row, null, false);
            }

            TextView colorView = row.FindViewById<TextView>(Resource.Id.colorSample);

            //string newColor = "#" +  colorList[position].R.ToString() + colorList[position].G.ToString() + colorList[position].B.ToString();

            colorView.SetBackgroundColor(HSVtoColor(colorList[position]));

            return row;
        }

        public Color HSVtoColor(Hsv hsvColor)
        {
            Color aColor;

            aColor = Color.HSVToColor(new float[]{
                (float) hsvColor.Hue * 2,
                (float) hsvColor.Satuation / 255,
                (float) hsvColor.Value / 255
            });

            return aColor;
        }
    }
}