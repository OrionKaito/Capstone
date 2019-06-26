package workflow.capstone.capstoneproject.adapter;

import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.Toast;

import java.util.List;

import workflow.capstone.capstoneproject.R;

public class ListUploadImageAdapter extends BaseAdapter {

    private Context mContext;
    private List<String> listImage;
    private LayoutInflater layoutInflater;

    public ListUploadImageAdapter(Context mContext, List<String> listImage) {
        this.mContext = mContext;
        this.listImage = listImage;
        layoutInflater = LayoutInflater.from(mContext);
    }

    @Override
    public int getCount() {
        return listImage.size();
    }

    @Override
    public Object getItem(int position) {
        return listImage.get(position);
    }

    @Override
    public long getItemId(int i) {
        return 0;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup viewGroup) {
        ViewHolder holder;
        if (convertView == null) {
            convertView = layoutInflater.inflate(R.layout.list_upload_image, null);
            holder = new ViewHolder();
            holder.imageItem = convertView.findViewById(R.id.image_item);
            convertView.setTag(holder);
        } else {
            holder = (ViewHolder) convertView.getTag();
        }

        String image = listImage.get(position);
        Bitmap imageBitmap = BitmapFactory.decodeFile(image);
        holder.imageItem.setImageBitmap(imageBitmap);
//        Toast.makeText(mContext, "image: " + image, Toast.LENGTH_SHORT).show();
        return convertView;
    }

    private class ViewHolder {
        ImageView imageItem;
    }
}
