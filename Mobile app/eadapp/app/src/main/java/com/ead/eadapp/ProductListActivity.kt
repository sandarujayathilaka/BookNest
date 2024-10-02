package com.ead.eadapp

import android.os.Bundle
import android.widget.Toast
import androidx.activity.ComponentActivity
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class ProductListActivity : ComponentActivity() {

    private lateinit var productAdapter: ProductAdapter
    private lateinit var recyclerView: RecyclerView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_product_list) // Ensure this points to your product list layout

        // Initialize RecyclerView
        recyclerView = findViewById(R.id.recyclerView)
        recyclerView.layoutManager = LinearLayoutManager(this)
        productAdapter = ProductAdapter(emptyList()) // Initialize with an empty list
        recyclerView.adapter = productAdapter

        // Fetch products
        fetchProducts()
    }

    private fun fetchProducts() {
        ApiClient.apiService.fetchProducts().enqueue(object : Callback<List<Product>> {
            override fun onResponse(call: Call<List<Product>>, response: Response<List<Product>>) {
                if (response.isSuccessful) {
                    response.body()?.let { products ->
                        productAdapter.updateProducts(products)
                    }
                } else {
                    Toast.makeText(this@ProductListActivity, "Failed to fetch products", Toast.LENGTH_SHORT).show()
                }
            }

            override fun onFailure(call: Call<List<Product>>, t: Throwable) {
                Toast.makeText(this@ProductListActivity, "Error: ${t.message}", Toast.LENGTH_SHORT).show()
            }
        })
    }
}
