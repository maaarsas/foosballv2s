using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using foosballv2s.Source.Activities.Helpers;
using foosballv2s.Source.Entities;
using Android.Graphics;

namespace foosballv2s.Source.Activities.Adapters
{
    /// <summary>
    /// This class populates a one row in the game list with needed values
    /// </summary>
    class ColorListAdapter : BaseAdapter<Color>
    {
        private List<Color> colorList;
        private Context cContext;
        public override int Count
        {
            get { return colorList.Count; }
        }

        public ColorListAdapter(Context context, List<Color> list)
        {
            colorList = list;
            cContext = context;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Color this[int position]
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

            colorView.SetBackgroundColor(colorList[position]);

            return row;
        }
    }
}