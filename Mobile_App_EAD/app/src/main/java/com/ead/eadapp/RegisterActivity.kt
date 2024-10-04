package com.ead.eadapp

import android.os.Bundle
import android.widget.Button
import android.widget.EditText
import android.widget.Toast
import androidx.activity.ComponentActivity
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

class RegisterActivity : ComponentActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_register)

        val registerButton = findViewById<Button>(R.id.registerButton)

        registerButton.setOnClickListener {
            val fullName = findViewById<EditText>(R.id.fullName).text.toString()
            val email = findViewById<EditText>(R.id.email).text.toString()
            val password = findViewById<EditText>(R.id.password).text.toString()

            val request = RegisterRequest(fullName, email, password)

            ApiClient.apiService.registerUser(request).enqueue(object : Callback<Void> {
                override fun onResponse(call: Call<Void>, response: Response<Void>) {
                    if (response.isSuccessful) {
                        Toast.makeText(this@RegisterActivity, "Registration successful!", Toast.LENGTH_LONG).show()
                    } else {
                        val errorBody = response.errorBody()?.string()
                        Toast.makeText(this@RegisterActivity,
                            "Registration failed: ${response.code()}. Error: $errorBody",
                            Toast.LENGTH_LONG).show()
                    }
                }

                override fun onFailure(call: Call<Void>, t: Throwable) {
                    Toast.makeText(this@RegisterActivity, "Error: ${t.message}", Toast.LENGTH_LONG).show()
                }
            })
        }
    }
}
