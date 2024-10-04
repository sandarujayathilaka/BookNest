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
    private lateinit var customerAddress: EditText
    private lateinit var customerNumber: EditText
    private lateinit var updateProfileButton: Button

    // Retrieve the token and email from Intent extras
    private val token: String by lazy {
        intent.getStringExtra("token") ?: ""
    }

    // This is the original email used for fetching and updating customer details.
    private val originalCustomerEmail: String by lazy {
        intent.getStringExtra("customer_email") ?: "sandarujayathilaka26@gmail.com"
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_profile)

        customerName = findViewById(R.id.customerName)
        customerEmail = findViewById(R.id.customerEmail)
        customerNumber = findViewById(R.id.customerNumber)
        customerAddress = findViewById(R.id.customerAddress)
        updateProfileButton = findViewById(R.id.updateProfileButton)

        // Disable email editing as it's typically a unique field
        customerEmail.setText(originalCustomerEmail)
        customerEmail.isEnabled = false

        // Fetch customer details when activity starts
        fetchCustomerDetails()

        // Set up button to update customer profile
        updateProfileButton.setOnClickListener {
            enableEditing()
        }
    }

    private fun fetchCustomerDetails() {
        ApiClient.apiService.fetchCustomerDetails("Bearer $token", originalCustomerEmail).enqueue(object : Callback<Customer> {
            override fun onResponse(call: Call<Customer>, response: Response<Customer>) {
                if (response.isSuccessful) {
                    Log.d("ProfileActivity", "Response: ${response.body().toString()}")

                    response.body()?.let { customer ->
                        customerName.setText(customer.fullName)
                        customerNumber.setText(customer.phoneNumber)
                        customerAddress.setText(customer.address)
                        customerEmail.setText(customer.email)
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
        customerAddress.isEnabled = true
        customerNumber.isEnabled = true
        customerEmail.isEnabled = true // Enable email editing since user can change it.
        updateProfileButton.text = "Save Changes"
        updateProfileButton.setOnClickListener {
            updateCustomerDetails()
        }
    }

    private fun updateCustomerDetails() {
        val updatedName = customerName.text.toString()
        val updatedAddress = customerAddress.text.toString()
        val updatedNumber = customerNumber.text.toString()
        val updatedEmail = customerEmail.text.toString()

        // Create an updated customer object with the new values
        val updatedCustomer = Customer(
            fullName = updatedName,
            address = updatedAddress,
            phoneNumber = updatedNumber,
            email = updatedEmail
        )

        // Use the original email as the path parameter to identify the user
        ApiClient.apiService.updateCustomerDetails("Bearer $token", originalCustomerEmail, updatedCustomer).enqueue(object : Callback<Void> {
            override fun onResponse(call: Call<Void>, response: Response<Void>) {
                if (response.isSuccessful) {
                    Toast.makeText(this@ProfileActivity, "Profile updated successfully", Toast.LENGTH_SHORT).show()
                    // Disable editing again
                    customerName.isEnabled = false
                    customerAddress.isEnabled = false
                    customerNumber.isEnabled = false
                    customerEmail.isEnabled = false // Disable email again
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

