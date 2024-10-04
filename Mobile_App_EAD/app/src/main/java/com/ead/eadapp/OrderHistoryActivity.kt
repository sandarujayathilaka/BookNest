package com.ead.eadapp

import android.os.Bundle
import android.util.Log
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class OrderHistoryActivity : AppCompatActivity() {

    private lateinit var orderRecyclerView: RecyclerView
    private lateinit var orderAdapter: OrderAdapter

    // Fetch customerId from intent
    private val customerId: String by lazy {
        intent.getStringExtra("customer_id") ?: "string"
    }

    // Tag for logging
    private val TAG = "OrderHistoryActivity"

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        Log.d(TAG, "onCreate: Starting OrderHistoryActivity")
        setContentView(R.layout.activity_order_history)

        orderRecyclerView = findViewById(R.id.orderRecyclerView)
        orderRecyclerView.layoutManager = LinearLayoutManager(this)

        orderAdapter = OrderAdapter(emptyList())
        orderRecyclerView.adapter = orderAdapter

        Log.d(TAG, "onCreate: RecyclerView initialized. Fetching order history...")
        fetchOrderHistory()
    }

    private fun fetchOrderHistory() {
        Log.d(TAG, "fetchOrderHistory: Fetching order history for customer ID: $customerId")

        ApiClient.apiService.fetchOrderHistory(customerId).enqueue(object : Callback<List<Order>> {
            override fun onResponse(call: Call<List<Order>>, response: Response<List<Order>>) {
                Log.d(TAG, "onResponse: Received response from API.")

                if (response.isSuccessful) {
                    Log.d(TAG, "onResponse: Response successful, processing orders...")
                    response.body()?.let { orders ->
                        orderAdapter.updateOrders(orders)
                        Log.d(TAG, "onResponse: Orders updated in RecyclerView.")
                    } ?: run {
                        Log.d(TAG, "onResponse: Response body is null.")
                    }
                } else {
                    Log.e(TAG, "onResponse: Failed to load order history. Code: ${response.code()}")
                    Toast.makeText(this@OrderHistoryActivity, "Failed to load order history", Toast.LENGTH_SHORT).show()
                }
            }

            override fun onFailure(call: Call<List<Order>>, t: Throwable) {
                Log.e(TAG, "onFailure: Error occurred: ${t.message}")
                Toast.makeText(this@OrderHistoryActivity, "Error: ${t.message}", Toast.LENGTH_SHORT).show()
            }
        })
    }
}
