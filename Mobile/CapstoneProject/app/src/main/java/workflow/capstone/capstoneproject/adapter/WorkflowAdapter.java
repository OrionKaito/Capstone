package workflow.capstone.capstoneproject.adapter;

import android.content.Context;
import android.graphics.Color;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

import java.util.List;

import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.entities.Workflow;
import workflow.capstone.capstoneproject.entities.WorkflowTemplate;

public class WorkflowAdapter extends BaseAdapter {

    private List<WorkflowTemplate> listData;
    private LayoutInflater layoutInflater;
    private Context mContext;

    public WorkflowAdapter(List<WorkflowTemplate> listData, Context mContext) {
        this.listData = listData;
        this.mContext = mContext;
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

    @Override
    public View getView(int position, View convertView, ViewGroup viewGroup) {
        ViewHolder holder;
        if (convertView == null) {
            convertView = layoutInflater.inflate(R.layout.workflow_listview, null);
            holder = new ViewHolder();
            holder.tvWorkflowName = convertView.findViewById(R.id.tv_workflow_name);
            holder.tvWorkflowDes = convertView.findViewById(R.id.tv_workflow_des);
            holder.lineView = convertView.findViewById(R.id.line_view);
            convertView.setTag(holder);
        } else {
            holder = (ViewHolder) convertView.getTag();
        }
        WorkflowTemplate workflowTemplate = this.listData.get(position);
        holder.tvWorkflowName.setText(workflowTemplate.getName());
        holder.tvWorkflowDes.setText(workflowTemplate.getDescription());
        if (position == getCount() - 1) {
            holder.lineView.setVisibility(View.GONE);
        } else {
            holder.lineView.setVisibility(View.VISIBLE);
        }
        return convertView;
    }

    private class ViewHolder {
        TextView tvWorkflowName;
        TextView tvWorkflowDes;
        View lineView;
    }
}
