package com.ead.eadapp

data class RegisterRequest(
    val fullName: String,
    val email: String,
    val passwordHash: String,
    val role: String = "Customer" // Default role is 'Customer'
)
