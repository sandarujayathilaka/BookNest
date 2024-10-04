package com.ead.eadapp

import android.content.Intent
import android.os.Bundle
import androidx.activity.ComponentActivity

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        // Start RegisterActivity directly
            val intent = Intent(this, LoginActivity::class.java)
        startActivity(intent)

        // Finish MainActivity so it can't be returned to
        finish()
    }
}
