package com.ead.eadapp

import android.os.Bundle
import android.util.Log
import android.widget.Button
import android.widget.EditText
import android.widget.Toast
import androidx.activity.ComponentActivity
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class ProfileActivity : ComponentActivity() {

    private lateinit var customerName: EditText
    private lateinit var customerEmail: EditText
    private lateinit var updateProfileButton: Button

    // Assume email is passed via intent or retrieved from shared preferences, etc.
    private val customerEmailValue: String by lazy {
        intent.getStringExtra("customer_email") ?: "sandarujayathilaka26@gmail.com"
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_profile)

        customerName = findViewById(R.id.customerName)
        customerEmail = findViewById(R.id.customerEmail)
        updateProfileButton = findViewById(R.id.updateProfileButton)

        // Disable email editing as it's typically a unique field
        customerEmail.setText(customerEmailValue)
        customerEmail.isEnabled = false

        // Fetch customer details when activity starts
        fetchCustomerDetails()

        // Set up button to update customer profile
        updateProfileButton.setOnClickListener {
            enableEditing()
        }
    }

    private fun fetchCustomerDetails() {
        ApiClient.apiService.fetchCustomerDetails(customerEmailValue).enqueue(object : Callback<Customer> {
            override fun onResponse(call: Call<Customer>, response: Response<Customer>) {
                Log.d("ProfileActivity", "Response: $response")
                if (response.isSuccessful) {
                    response.body()?.let { customer ->
                        customerName.setText(customer.fullName)
                    }
                } else {
                    Toast.makeText(this@ProfileActivity, "Failed to load profile", Toast.LENGTH_SHORT).show()
                }
            }

            override fun onFailure(call: Call<Customer>, t: Throwable) {
                Toast.makeText(this@ProfileActivity, "Error: ${t.message}", Toast.LENGTH_SHORT).show()
            }
        })
    }

    private fun enableEditing() {
        customerName.isEnabled = true

        updateProfileButton.text = "Save Changes"
        updateProfileButton.setOnClickListener {
            updateCustomerDetails()
        }
    }

    private fun updateCustomerDetails() {
        val updatedName = customerName.text.toString()

        val updatedCustomer = Customer(updatedName, customerEmailValue)

        ApiClient.apiService.updateCustomerDetails(customerEmailValue, updatedCustomer).enqueue(object : Callback<Void> {
            override fun onResponse(call: Call<Void>, response: Response<Void>) {
                if (response.isSuccessful) {
                    Toast.makeText(this@ProfileActivity, "Profile updated successfully", Toast.LENGTH_SHORT).show()
                    // Disable editing again
                    customerName.isEnabled = false
                    updateProfileButton.text = "Update Profile"
                } else {
                    Toast.makeText(this@ProfileActivity, "Failed to update profile", Toast.LENGTH_SHORT).show()
                }
            }

            override fun onFailure(call: Call<Void>, t: Throwable) {
                Toast.makeText(this@ProfileActivity, "Error: ${t.message}", Toast.LENGTH_SHORT).show()
            }
        })
    }
}
