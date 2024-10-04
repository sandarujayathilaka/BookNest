package com.ead.eadapp

import android.content.Intent
import android.os.Bundle
import androidx.activity.ComponentActivity

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        val customerId = "string" // Replace with actual customer ID
        val intent = Intent(this, CurrentOrderActivity::class.java)
        intent.putExtra("customer_id", customerId)
        startActivity(intent)
        finish()

        // Start RegisterActivity directly
//        val intent = Intent(this, OrderHistoryActivity::class.java)
//        startActivity(intent)
        //finish()
    }
}
