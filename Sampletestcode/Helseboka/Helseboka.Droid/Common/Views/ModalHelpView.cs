
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Helseboka.Core.Common.EnumDefinitions;
using Helseboka.Core.Common.Model;
using Helseboka.Core.Common.Extension;
using Helseboka.Core.HelpAndFAQ.Model;
using Helseboka.Droid.Common.Adapters;

namespace Helseboka.Droid.Common.Views
{
    public class ModalHelpView : IUniversalAdapter
    {
        private Dialog dialog;
        private RecyclerView helpDataListView;
        private ImageView closeButton;
        private Context context;
        private HelpFAQType helpType;
        private List<HelpFAQDataModel> helpDataList;

        public ModalHelpView(Context context, HelpFAQType helpType)
        {
            this.context = context;
            this.helpType = helpType;
        }

        //public override void OnCreate(Bundle savedInstanceState)
        //{
        //    base.OnCreate(savedInstanceState);
        //    SetStyle(DialogFragmentStyle.Normal, Resource.Style.dialog_help_style);
        //}

        //public override void OnStart()
        //{
        //    base.OnStart();

        //    if (Dialog != null)
        //    {
        //        Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
        //    }
        //}

        //public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        //{
        //    base.OnCreateView(inflater, container, savedInstanceState);

        //    var view = inflater.Inflate(Resource.Layout.fragment_modal_help, null);

        //    helpDataListView = view.FindViewById<RecyclerView>(Resource.Id.helpListView);
        //    closeButton = view.FindViewById<ImageView>(Resource.Id.closeButton);

        //    closeButton.Click += CloseButton_Click;

        //    var layoutManager = new LinearLayoutManager(context, LinearLayoutManager.Vertical, false);
        //    helpDataListView.SetLayoutManager(layoutManager);
        //    helpDataListView.SetAdapter(new GenericRecyclerAdapter(this));

        //    LoadData().Forget();

        //    return view;
        //}

        public async Task Show()
        {
            var inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);

            var view = inflater.Inflate(Resource.Layout.fragment_modal_help, null);

            helpDataListView = view.FindViewById<RecyclerView>(Resource.Id.helpListView);
            closeButton = view.FindViewById<ImageView>(Resource.Id.closeButton);

            closeButton.Click += CloseButton_Click;

            var layoutManager = new LinearLayoutManager(context, LinearLayoutManager.Vertical, false);
            helpDataListView.SetLayoutManager(layoutManager);
            helpDataListView.SetAdapter(new GenericRecyclerAdapter(this));

            dialog = new Dialog(context, Android.Resource.Style.ThemeLightNoTitleBar);
            dialog.SetContentView(view);
            dialog.SetCancelable(false);
            await LoadData();
            dialog.Show();
        }

        private async Task LoadData()
        {
            helpDataList = await new HelpFAQManager().GetHelpFAQList(helpType);
            if (helpDataList != null && helpDataList.Count > 0)
            {
                helpDataListView.GetAdapter().NotifyDataSetChanged();
            }
        }

        void CloseButton_Click(object sender, EventArgs e)
        {
            if (dialog != null)
            {
                dialog.Dismiss();
            }
        }

        public int GetItemCount()
        {
            return helpDataList != null ? helpDataList.Count : 0;
        }

        public UniversalViewHolder CreateItemView(ViewGroup parent, int viewType)
        {
            var inflater = LayoutInflater.FromContext(context);
            var view = inflater.Inflate(Resource.Layout.template_modal_help, null);

            var viewMap = new Dictionary<int, View>();
            viewMap.Add(Resource.Id.helpTitle, view.FindViewById(Resource.Id.helpTitle));
            viewMap.Add(Resource.Id.helpDescription, view.FindViewById(Resource.Id.helpDescription));

            return new UniversalViewHolder(view, viewMap);
        }

        public void BindViewHolder(UniversalViewHolder holder, int position)
        {
            var data = helpDataList[position];

            holder.GetView<TextView>(Resource.Id.helpTitle).Text = data.Title;
            holder.GetView<TextView>(Resource.Id.helpDescription).Text = data.Description;
        }

        public void OnItemClick(int position)
        {
            
        }

    }
}
