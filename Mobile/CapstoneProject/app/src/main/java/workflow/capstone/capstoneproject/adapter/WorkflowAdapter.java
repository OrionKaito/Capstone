package workflow.capstone.capstoneproject.adapter;

import android.content.Context;
import android.graphics.Color;
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
import workflow.capstone.capstoneproject.entities.Workflow;
import workflow.capstone.capstoneproject.entities.WorkflowTemplate;

public class WorkflowAdapter extends BaseAdapter {

    private List<WorkflowTemplate> listData;
    private LayoutInflater layoutInflater;
    private Context mContext;
    private String createDate;

    public WorkflowAdapter(List<WorkflowTemplate> listData, Context mContext) {
        this.listData = listData;
        this.mContext = mContext;
        layoutInflater = LayoutInflater.from(mContext);
    }

    public void AddListItemAdapter(List<WorkflowTemplate> listDataPlus) {
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
    public View getView(int position, View convertView, ViewGroup viewGroup) {
        ViewHolder holder;
        if (convertView == null) {
            convertView = layoutInflater.inflate(R.layout.workflow_listview, null);
            holder = new ViewHolder();
            holder.tvWorkflowName = convertView.findViewById(R.id.tv_workflow_name);
            holder.tvWorkflowDes = convertView.findViewById(R.id.tv_workflow_des);
            holder.tvWorkflowCreateDate = convertView.findViewById(R.id.tv_workflow_create_date);
            convertView.setTag(holder);
        } else {
            holder = (ViewHolder) convertView.getTag();
        }

        WorkflowTemplate workflowTemplate = this.listData.get(position);
        holder.tvWorkflowName.setText(workflowTemplate.getName());
        holder.tvWorkflowDes.setText(workflowTemplate.getDescription());
        try {
            Date date = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss").parse(workflowTemplate.getCreateDate());
            createDate = new SimpleDateFormat("MMM dd yyyy").format(date);
        } catch (ParseException e) {
            e.printStackTrace();
        }
        holder.tvWorkflowCreateDate.setText(createDate);
        return convertView;
    }

    private class ViewHolder {
        TextView tvWorkflowName;
        TextView tvWorkflowDes;
        TextView tvWorkflowCreateDate;
    }
}
