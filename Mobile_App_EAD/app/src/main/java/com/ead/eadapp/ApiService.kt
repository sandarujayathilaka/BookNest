package com.ead.eadapp

import retrofit2.http.GET
import retrofit2.Call
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import retrofit2.http.Body
import retrofit2.http.Header
import retrofit2.http.POST
import retrofit2.http.PUT
import retrofit2.http.Path

// Define the API endpoints
interface ApiService {
    @POST("api/User/register")
    fun registerUser(@Body request: RegisterRequest): Call<Void>

    @GET("api/Product")  // Adjust the URL to your actual ASP.NET API route for fetching products
    fun fetchProducts(): Call<List<Product>>

    @GET("api/User/{email}")
    fun fetchCustomerDetails(@Path("email") email: String): Call<Customer>

    @PUT("api/User/{email}")
    fun updateCustomerDetails(@Path("email") email: String, @Body customer: Customer): Call<Void>

}

// Singleton object to initialize Retrofit and create the ApiService instance
object ApiClient {
    private const val BASE_URL = "http://10.0.2.2:5271/"  // Adjust this as needed

    private val retrofit: Retrofit by lazy {
        Retrofit.Builder()
            .baseUrl(BASE_URL)
            .addConverterFactory(GsonConverterFactory.create())
            .build()
    }

    val apiService: ApiService by lazy {
        retrofit.create(ApiService::class.java)
    }
}
