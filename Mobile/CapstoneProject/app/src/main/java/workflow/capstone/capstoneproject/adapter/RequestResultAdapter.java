package workflow.capstone.capstoneproject.adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;

import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.entities.StaffResult;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowUtils;

public class RequestResultAdapter extends BaseAdapter {

    private Context mContext;
    private List<StaffResult> staffResultList;
    private LayoutInflater layoutInflater;

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

    @Override
    public View getView(final int position, View convertView, ViewGroup viewGroup) {
        ViewHolder holder;
        if (convertView == null) {
            convertView = layoutInflater.inflate(R.layout.item_request_result, null);
            holder = new ViewHolder();
            holder.tvStaffName = convertView.findViewById(R.id.tv_staff_name);
            holder.tvUsername = convertView.findViewById(R.id.tv_username);
            holder.tvCreateDate = convertView.findViewById(R.id.tv_create_date);
            holder.tvStatus = convertView.findViewById(R.id.tv_status);
//            holder.lineView = convertView.findViewById(R.id.line_view);
            convertView.setTag(holder);
        } else {
            holder = (ViewHolder) convertView.getTag();
        }

        StaffResult staffResult = staffResultList.get(position);
        holder.tvStaffName.setText(staffResult.getFullName());
        holder.tvUsername.setText("( " + staffResult.getUserName() + " )");

//        if (position == 0) {
//            holder.lineView.setVisibility(View.VISIBLE);
//        }

        String createDate = "";
        try {
            Date date = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss").parse(staffResult.getCreateDate());
            createDate = new SimpleDateFormat("MMM dd yyyy' at 'hh:mm a").format(date);
        } catch (ParseException e) {
            e.printStackTrace();
        }

        if(!createDate.equals("")) {
            holder.tvCreateDate.setText(createDate);
        }
        holder.tvStatus.setText(staffResult.getStatus());
        return convertView;
    }

    private class ViewHolder {
        TextView tvStaffName;
        TextView tvUsername;
        TextView tvCreateDate;
        TextView tvStatus;
//        View lineView;
    }
}
