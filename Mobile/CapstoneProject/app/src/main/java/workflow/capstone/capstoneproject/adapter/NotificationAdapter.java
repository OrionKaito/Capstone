package workflow.capstone.capstoneproject.adapter;

import android.content.Context;
import android.os.Build;
import android.support.annotation.RequiresApi;
import android.support.v4.text.HtmlCompat;
import android.text.Html;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;

import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.entities.UserNotification;


public class NotificationAdapter extends BaseAdapter {

    private List<UserNotification> listData;
    private LayoutInflater layoutInflater;
    private Context mContext;
    private String createDate;
    private boolean check;

    public NotificationAdapter(List<UserNotification> listData, Context mContext, boolean mCheck) {
        this.listData = listData;
        this.mContext = mContext;
        this.check = mCheck;
        layoutInflater = LayoutInflater.from(mContext);
    }

    @Override
    public int getCount() {
        return listData.size();
    }

    @Override
    public Object getItem(int position) {
        return listData.get(position);
    }

    @Override
    public long getItemId(int position) {
        return position;
    }

    @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
    @Override
    public View getView(int position, View convertView, ViewGroup viewGroup) {
        ViewHolder holder;
        if (convertView == null) {
            convertView = layoutInflater.inflate(R.layout.notification_listview, null);
            holder = new ViewHolder();
            holder.tvWorkflowName = convertView.findViewById(R.id.tv_workflow_name);
            holder.tvMessage = convertView.findViewById(R.id.tv_message);
            holder.tvCreateDate = convertView.findViewById(R.id.tv_create_date);
            holder.imgIsRead = convertView.findViewById(R.id.img_is_read);
            convertView.setTag(holder);
        } else {
            holder = (ViewHolder) convertView.getTag();
        }

        UserNotification userNotification = this.listData.get(position);

        holder.tvWorkflowName.setText(userNotification.getWorkflowName());

        String message = userNotification.getMessage().contains("completed") ? userNotification.getMessage() : userNotification.getMessage() + " from " + "<b>" + userNotification.getActorName() + "</b>";
        holder.tvMessage.setText(HtmlCompat.fromHtml(message, Html.FROM_HTML_MODE_LEGACY));

        try {
            Date date = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss").parse(userNotification.getCreateDate());
            createDate = new SimpleDateFormat("MMM dd yyyy' at 'hh:mm a").format(date);
        } catch (ParseException e) {
            e.printStackTrace();
        }
        holder.tvCreateDate.setText(createDate);

        if(check) {
            if (userNotification.getIsRead()) {
                holder.imgIsRead.setVisibility(View.VISIBLE);
                holder.imgIsRead.setImageResource(R.drawable.ic_lens_gray_24dp);
            } else {
                holder.imgIsRead.setVisibility(View.VISIBLE);
                holder.imgIsRead.setImageResource(R.drawable.ic_lens_green_24dp);
            }
        } else {
            holder.imgIsRead.setVisibility(View.GONE);
        }

        return convertView;
    }

    private class ViewHolder {
        TextView tvWorkflowName;
        TextView tvMessage;
        TextView tvCreateDate;
        ImageView imgIsRead;
    }
}
