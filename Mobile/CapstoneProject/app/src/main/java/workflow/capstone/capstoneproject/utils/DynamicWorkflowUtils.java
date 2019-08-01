package workflow.capstone.capstoneproject.utils;

import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListAdapter;
import android.widget.ListView;
import android.widget.ProgressBar;

import workflow.capstone.capstoneproject.R;

public class DynamicWorkflowUtils {

    private static final String[] okFileExtensions =  new String[] {"jpg", "png", "gif","jpeg"};

    public static boolean isConnectingToInternet(Context context) {
        ConnectivityManager connectivity = (ConnectivityManager) context
                .getSystemService(Context.CONNECTIVITY_SERVICE);
        if (connectivity != null) {
            NetworkInfo[] info = connectivity.getAllNetworkInfo();
            if (info != null)
                for (int i = 0; i < info.length; i++) {
                    if (info[i].getState() == NetworkInfo.State.CONNECTED) {
                        return true;
                    }
                }
        }
        return false;
    }

    public static void setListViewHeightBasedOnChildren(ListView listView) {
        ListAdapter listAdapter = listView.getAdapter();
        if (listAdapter == null) {
            // pre-condition
            return;
        }

        int totalHeight = 0;
        int desiredWidth = View.MeasureSpec.makeMeasureSpec(listView.getWidth(), View.MeasureSpec.AT_MOST);
        for (int i = 0; i < listAdapter.getCount(); i++) {
            View listItem = listAdapter.getView(i, null, listView);
            listItem.measure(desiredWidth, View.MeasureSpec.UNSPECIFIED);
            totalHeight += listItem.getMeasuredHeight();
        }

        ViewGroup.LayoutParams params = listView.getLayoutParams();
        params.height = totalHeight + (listView.getDividerHeight() * (listAdapter.getCount() - 1));
        listView.setLayoutParams(params);
        listView.requestLayout();
    }

    public static void addListViewFooter(Context context, ListView listView){
        View view = LayoutInflater.from(context).inflate(R.layout.footer_listview_progressbar, null);
//        ProgressBar progressBar = view.findViewById(R.id.progress_bar);
        listView.addFooterView(view);
    }

    public static void removeListViewFooter(Context context, ListView listView){
        View view = LayoutInflater.from(context).inflate(R.layout.footer_listview_progressbar, null);
//        ProgressBar progressBar = view.findViewById(R.id.progress_bar);
        listView.removeFooterView(view);
    }

    public static boolean accept(String imageExtension)
    {
        for (String extension : okFileExtensions)
        {
            if (imageExtension.toLowerCase().equals(extension))
            {
                return true;
            }
        }
        return false;
    }

    public static String mapNameWithUserName(String fullName, String userName) {
        return fullName + " ( " + userName + " ) ";
    }
}
