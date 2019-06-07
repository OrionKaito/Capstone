package workflow.capstone.capstoneproject.utils;

import android.app.Activity;
import android.content.Context;

import com.kaopiz.kprogresshud.KProgressHUD;

public class KProgressHUDManager {
    public static KProgressHUD showProgressBar(Context context) {
        if (!((Activity) (context)).isFinishing()) {
            KProgressHUD hud = KProgressHUD.create(context)
                    .setStyle(KProgressHUD.Style.SPIN_INDETERMINATE)
                    .setCancellable(true)
                    .setAnimationSpeed(2)
                    .setDimAmount(0.5f)
                    .show();
            return hud;
        }
        return null;
    }

    public static void dismiss(Context context, KProgressHUD hud) {
        if (hud != null && hud.isShowing() && !((Activity) (context)).isFinishing()) {
            hud.dismiss();
        }
    }
}
