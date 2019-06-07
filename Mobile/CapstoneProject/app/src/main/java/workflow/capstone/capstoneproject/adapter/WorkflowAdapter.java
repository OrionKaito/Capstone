package workflow.capstone.capstoneproject.adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

import java.util.List;

import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.entities.Workflow;

public class WorkflowAdapter extends BaseAdapter {

    private List<Workflow> listData;
    private LayoutInflater layoutInflater;
    private Context mContext;

    public WorkflowAdapter(List<Workflow> listData, Context mContext) {
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
            holder.tvWorkflowName = convertView.findViewById(R.id.tvWorkflowName);
            holder.tvWorkflowDes = convertView.findViewById(R.id.tvWorkflowDes);
            convertView.setTag(holder);
        } else {
            holder = (ViewHolder) convertView.getTag();
        }
        Workflow workflow = this.listData.get(position);
        holder.tvWorkflowName.setText(workflow.getName());
        holder.tvWorkflowDes.setText(workflow.getDescription());
        return convertView;
    }

    private class ViewHolder {
        TextView tvWorkflowName;
        TextView tvWorkflowDes;
    }
}
