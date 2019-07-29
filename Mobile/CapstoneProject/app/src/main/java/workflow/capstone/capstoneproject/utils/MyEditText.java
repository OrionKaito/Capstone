package workflow.capstone.capstoneproject.utils;

import android.content.Context;
import android.graphics.Canvas;
import android.graphics.Paint;
import android.util.AttributeSet;
import android.widget.EditText;

import workflow.capstone.capstoneproject.R;

public class MyEditText extends android.support.v7.widget.AppCompatEditText {
    private Paint mPaint = new Paint();

    public MyEditText(Context context) {
        super(context);
        initPaint();
    }

    public MyEditText(Context context, AttributeSet attrs) {
        super(context, attrs);
        initPaint();
    }

    public MyEditText(Context context, AttributeSet attrs, int defStyle) {
        super(context, attrs, defStyle);
        initPaint();
    }

    private void initPaint() {
        mPaint.setStyle(Paint.Style.STROKE);
        mPaint.setColor(getResources().getColor(R.color.colortext));
    }

    @Override protected void onDraw(Canvas canvas) {
        int left = getLeft();
        int right = getRight();
        int paddingTop = getPaddingTop();
        int paddingBottom = getPaddingBottom();
        int paddingLeft = getPaddingLeft();
        int paddingRight = getPaddingRight();
        int height = getHeight();
        int lineHeight = getLineHeight();
        int count = (height-paddingTop-paddingBottom) / lineHeight;

        for (int i = 0; i < count; i++) {
            int baseline = lineHeight * (i+1) + paddingTop;
            canvas.drawLine(left+paddingLeft, baseline, right-paddingRight, baseline, mPaint);
        }

        super.onDraw(canvas);
    }
}
