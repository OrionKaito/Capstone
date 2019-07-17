package workflow.capstone.capstoneproject.adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

import java.util.List;

import workflow.capstone.capstoneproject.R;
import workflow.capstone.capstoneproject.entities.Comment;

public class CommentAdapter extends BaseAdapter {

    private List<Comment> listData;
    private LayoutInflater layoutInflater;
    private Context mContext;

    public CommentAdapter(List<Comment> listData, Context mContext) {
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
            convertView = layoutInflater.inflate(R.layout.list_comment, null);
            holder = new ViewHolder();
            holder.tvComment = convertView.findViewById(R.id.tv_comment);
            holder.tvFullName = convertView.findViewById(R.id.tv_full_name);
            convertView.setTag(holder);
        } else {
            holder = (ViewHolder) convertView.getTag();
        }

        Comment comment = this.listData.get(position);

        holder.tvFullName.setText(comment.getFullName());
        holder.tvComment.setText(comment.getComment());
        return convertView;
    }

    private class ViewHolder {
        TextView tvComment;
        TextView tvFullName;
    }
}
