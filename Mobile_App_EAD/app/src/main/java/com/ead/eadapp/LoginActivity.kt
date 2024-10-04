package com.ead.eadapp

import android.content.Intent
import android.os.Bundle
import android.text.InputType
import android.widget.Button
import android.widget.CheckBox
import android.widget.EditText
import android.widget.ImageView
import android.widget.TextView
import android.widget.Toast
import androidx.activity.ComponentActivity
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response


class LoginActivity: ComponentActivity() {
    private var isPasswordVisible = false
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_login)

        val loginButton = findViewById<Button>(R.id.btnLogin)
        val emailEditText = findViewById<EditText>(R.id.etUsername)
        val passwordEditText = findViewById<EditText>(R.id.etPassword)
        val togglePasswordVisibilityImageView = findViewById<ImageView>(R.id.ivTogglePasswordVisibility)
        val forgetPasswordTextView = findViewById<TextView>(R.id.tvForgotPassword)


        loginButton.setOnClickListener {
            val email = emailEditText.text.toString()
            val password = passwordEditText.text.toString()

            if (email.isEmpty() && password.isEmpty()) {
                Toast.makeText(this, "Please enter both email address and password", Toast.LENGTH_LONG).show()
                return@setOnClickListener
            }
            // Validate email format
            if (!android.util.Patterns.EMAIL_ADDRESS.matcher(email).matches()) {
                Toast.makeText(this, "Please enter a valid email address", Toast.LENGTH_LONG).show()
                return@setOnClickListener
            }

            // Validate inputs

            if (password.isEmpty()) {
                Toast.makeText(this, "Please enter password", Toast.LENGTH_LONG).show()
                return@setOnClickListener
            }

            // Call the backend login API
            val request = LoginRequest(email, password)
            ApiClient.apiService.loginUser(request).enqueue(object : Callback<LoginResponse> {
                override fun onResponse(
                    call: Call<LoginResponse>,
                    response: Response<LoginResponse>
                ) {
                    if (response.isSuccessful) {
                        val loginResponse = response.body()
                        // Handle login success
                        Toast.makeText(this@LoginActivity, "Login successful!", Toast.LENGTH_LONG).show()

                        // Assuming `token` and `customer_email` are part of the LoginResponse
                        val token = loginResponse?.token ?: ""
                        val customerEmail = loginResponse?.email  ?: email // Replace with the actual field for email if necessary

                        // Create an Intent to start ProfileActivity
                        val intent = Intent(this@LoginActivity, ProfileActivity::class.java).apply {
                            putExtra("token", token)
                            putExtra("customer_email", customerEmail)
                        }

                        // Start ProfileActivity
                        startActivity(intent)
                        finish() // Optional: Close LoginActivity so it can't be returned to
                    } else {
                        val errorBody = response.errorBody()?.string()
                        Toast.makeText(
                            this@LoginActivity,
                            "Login failed: ${response.code()}. Error: $errorBody",
                            Toast.LENGTH_LONG
                        ).show()
                    }
                }


                override fun onFailure(call: Call<LoginResponse>, t: Throwable) {
                    Toast.makeText(this@LoginActivity, "Error: ${t.message}", Toast.LENGTH_LONG).show()
                }
            })
        }

        togglePasswordVisibilityImageView.setOnClickListener {
            isPasswordVisible = !isPasswordVisible // Toggle the visibility state
            if (isPasswordVisible) {
                // Show password
                passwordEditText.inputType = InputType.TYPE_TEXT_VARIATION_VISIBLE_PASSWORD
                togglePasswordVisibilityImageView.setImageResource(R.drawable.ic_visibility) // Show visible icon
            } else {
                // Hide password
                passwordEditText.inputType = InputType.TYPE_CLASS_TEXT or InputType.TYPE_TEXT_VARIATION_PASSWORD
                togglePasswordVisibilityImageView.setImageResource(R.drawable.ic_visibility_off) // Show hidden icon
            }
            passwordEditText.setSelection(passwordEditText.text.length) // Keep cursor at the end
        }


        // Forget password click
        forgetPasswordTextView.setOnClickListener {
            val intent = Intent(this, ForgetPasswordActivity::class.java)
            startActivity(intent)
        }
    }
}