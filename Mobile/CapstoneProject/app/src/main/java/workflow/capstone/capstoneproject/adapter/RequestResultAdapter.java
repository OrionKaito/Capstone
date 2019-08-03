package workflow.capstone.capstoneproject.adapter;

import android.annotation.SuppressLint;
import android.content.Context;
import android.graphics.Color;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.BaseAdapter;
import android.widget.ListView;
import android.widget.TextView;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Random;

import de.hdodenhof.circleimageview.CircleImageView;
import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.entities.Comment;
import workflow.capstone.capstoneproject.entities.RequestValue;
import workflow.capstone.capstoneproject.entities.StaffResult;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowUtils;

public class RequestResultAdapter extends BaseAdapter {

    private Context mContext;
    private List<StaffResult> staffResultList;
    private LayoutInflater layoutInflater;
    private CommentAdapter commentAdapter;

    public RequestResultAdapter(Context mContext, List<StaffResult> staffResultList) {
        this.mContext = mContext;
        this.staffResultList = staffResultList;
        layoutInflater = LayoutInflater.from(mContext);
    }

    @Override
    public int getCount() {
        return staffResultList.size();
    }

    @Override
    public Object getItem(int position) {
        return staffResultList.get(position);
    }

    @Override
    public long getItemId(int i) {
        return 0;
    }

    @SuppressLint("ResourceType")
    @Override
    public View getView(final int position, View convertView, ViewGroup viewGroup) {
        ViewHolder holder;
        if (convertView == null) {
            convertView = layoutInflater.inflate(R.layout.item_request_result, null);
            holder = new ViewHolder();
            holder.tvStaffName = convertView.findViewById(R.id.tv_staff_name);
            holder.tvStaffUsername = convertView.findViewById(R.id.tv_staff_username);
            holder.tvDateApprove = convertView.findViewById(R.id.tv_date_approve);
            holder.tvStatus = convertView.findViewById(R.id.tv_status);
            holder.lvComment = convertView.findViewById(R.id.lv_comment);
            convertView.setTag(holder);
        } else {
            holder = (ViewHolder) convertView.getTag();
        }

        StaffResult staffResult = staffResultList.get(position);
        holder.tvStaffName.setText(staffResult.getFullName());
        holder.tvStaffUsername.setText("( " + staffResult.getUserName() + " )");

        String dateApprove = "";
        try {
            Date date = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss").parse(staffResult.getCreateDate());
            dateApprove = new SimpleDateFormat("MMM dd yyyy' at 'hh:mm a").format(date);
        } catch (ParseException e) {
            e.printStackTrace();
        }

        if (!dateApprove.equals("")) {
            holder.tvDateApprove.setText(dateApprove);
        }
        holder.tvStatus.setText(staffResult.getStatus());

        List<Comment> commentList = new ArrayList<>();
        for (RequestValue requestValue : staffResult.getRequestValues()) {
            commentList.add(new Comment(requestValue.getValue(), staffResult.getFullName(), dateApprove));
        }

        commentAdapter = new CommentAdapter(commentList, mContext);

        commentAdapter.notifyDataSetChanged();
        holder.lvComment.setAdapter(commentAdapter);
        holder.lvComment.setClickable(false);
        DynamicWorkflowUtils.setListViewHeightBasedOnChildren(holder.lvComment);
        return convertView;
    }

    private class ViewHolder {
        TextView tvStaffName;
        TextView tvStaffUsername;
        TextView tvDateApprove;
        TextView tvStatus;
        ListView lvComment;
    }
}
