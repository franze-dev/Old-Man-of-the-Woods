package com.example.logPlugin;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.util.Scanner;
public class LogManager {
    private static  final String FileName = "logs.txt";

    private Context context;
    private Activity activity;

    public LogManager() {

    }

    public void sendLog(String log) {
        if (context == null) {
            android.util.Log.e("LogManager", "Context is null - cannot save log");
            return;
        }

        try {
            File file = new File(context.getFilesDir(), FileName);
            FileWriter writer = new FileWriter(file, true);
            writer.append(log).append("\n");
            writer.close();
        }
        catch (IOException e) {
            e.printStackTrace();
        }
    }

    public String getLogs()
    {
        StringBuilder logs = new StringBuilder();
        try {
            File file = new File(context.getFilesDir(), FileName);
            if (!file.exists()) return  "NO FILE FOUND. FILE: " + FileName;

            Scanner scanner = new Scanner(file);

            while (scanner.hasNextLine()) {
                logs.append(scanner.nextLine()).append("\n");
            }

            scanner.close();
        }
        catch (Exception e) {
            e.printStackTrace();
        }
        return logs.toString();
    }

    public void clearLogs() {

        if (activity == null || context == null)
        {
            android.util.Log.e("LogManager", "Activity/Context are null");
            return;
        }

        activity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                AlertDialog.Builder builder = new AlertDialog.Builder(activity);
                builder.setTitle("Confirm Delete")
                        .setMessage("Are you sure you want to delete all logs?")
                        .setPositiveButton("Yes", new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                File file = new File(context.getFilesDir(), FileName);
                                if (file.exists()) {
                                    boolean deleted = file.delete();
                                    android.util.Log.d("LogManager", "Logs deleted: " + deleted);
                                } else {
                                    android.util.Log.d("LogManager", "Log file did not exist");
                                }
                                dialog.dismiss();
                            }
                        })
                        .setNegativeButton("No", new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                dialog.dismiss();
                            }
                        })
                        .setCancelable(false)
                        .show();
            }
        });
    }

    public void setActivity(Activity activity) {
        this.activity = activity;
        this.context = activity.getApplicationContext();
    }

    public static LogManager create(Activity activity) {
        LogManager manager = new LogManager();
        manager.setActivity(activity);
        return manager;
    }
}
