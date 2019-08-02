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
import workflow.capstone.capstoneproject.entities.MyRequest;

public class MyRequestAdapter extends BaseAdapter {

    private List<MyRequest> listData;
    private LayoutInflater layoutInflater;
    private Context mContext;

    public MyRequestAdapter(List<MyRequest> listData, Context mContext) {
        this.listData = listData;
        this.mContext = mContext;
        layoutInflater = LayoutInflater.from(mContext);
    }

    public void AddListItemAdapter(List<MyRequest> listDataPlus) {
        listData.addAll(listDataPlus);
        this.notifyDataSetChanged();
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

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        ViewHolder holder;
        if (convertView == null) {
            convertView = layoutInflater.inflate(R.layout.item_my_request, null);
            holder = new ViewHolder();
            holder.tvWorkflowName = convertView.findViewById(R.id.tv_workflow_name);
            holder.tvCurrentRequestAction = convertView.findViewById(R.id.tv_current_request_action);
            holder.tvComplete = convertView.findViewById(R.id.tv_complete);
            holder.tvCreateDate = convertView.findViewById(R.id.tv_create_date);
            convertView.setTag(holder);
        } else {
            holder = (ViewHolder) convertView.getTag();
        }

        MyRequest myRequest = this.listData.get(position);
        holder.tvWorkflowName.setText(myRequest.getWorkFlowTemplateName());
        holder.tvCurrentRequestAction.setText(myRequest.getCurrentRequestActionName());

        String createDate = "";

        try {
            Date date = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss").parse(myRequest.getCreateDate());
            createDate = new SimpleDateFormat("MMM dd yyyy' at 'hh:mm a").format(date);
        } catch (ParseException e) {
            e.printStackTrace();
        }

        holder.tvCreateDate.setText(createDate);

        if (myRequest.getIsCompleted()) {
            holder.tvComplete.setText("Completed!");
        } else {
            holder.tvComplete.setText("Pending...");
        }

        return convertView;
    }

    private class ViewHolder {
        TextView tvWorkflowName;
        TextView tvCurrentRequestAction;
        TextView tvComplete;
        TextView tvCreateDate;
    }
}
