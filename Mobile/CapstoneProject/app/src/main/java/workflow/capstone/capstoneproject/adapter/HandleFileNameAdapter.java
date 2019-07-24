package workflow.capstone.capstoneproject.adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.LinearLayout;
import android.widget.TextView;

import java.util.List;

import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.utils.DynamicWorkflowUtils;

public class HandleFileNameAdapter extends BaseAdapter {

    private Context mContext;
    private List<String> listFileName;
    private LayoutInflater layoutInflater;

    public HandleFileNameAdapter(Context mContext, List<String> listFileName) {
        this.mContext = mContext;
        this.listFileName = listFileName;
        layoutInflater = LayoutInflater.from(mContext);
    }

    @Override
    public int getCount() {
        return listFileName.size();
    }

    @Override
    public Object getItem(int position) {
        return listFileName.get(position);
    }

    @Override
    public long getItemId(int i) {
        return 0;
    }

    @Override
    public View getView(final int position, View convertView, ViewGroup viewGroup) {
        ViewHolder holder;
        if (convertView == null) {
            convertView = layoutInflater.inflate(R.layout.item_file_name, null);
            holder = new ViewHolder();
            holder.tvFileName = convertView.findViewById(R.id.tv_file_name);
            convertView.setTag(holder);
        } else {
            holder = (ViewHolder) convertView.getTag();
        }

        String fileName = listFileName.get(position);
        holder.tvFileName.setText(fileName);
        if (!DynamicWorkflowUtils.accept(fileName.substring(fileName.lastIndexOf(".") +1 ))) {
            holder.tvFileName.setCompoundDrawablesWithIntrinsicBounds(0,0,R.drawable.ic_download,0);
        }

        return convertView;
    }

    private class ViewHolder {
        TextView tvFileName;
    }
}
