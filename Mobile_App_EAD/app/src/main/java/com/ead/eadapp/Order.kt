package com.ead.eadapp

data class Order(
    val id: String,
    val orderID: String,
    val products: List<ProductOrder>,
    val customerID: String,
    val orderDate: String,
    val status: String,
    val totalItems: Int,
    val orderCancelation: Boolean,
    val cancelationNote: String,
    val totalAmount: Double,
    val lastStatusChange: String,
    val cancelationOfficerNote: String?
)

data class ProductOrder(
    val productID: String,
    val vendorID: String,
    val status: String,
    val totalItems: Int,
    val unitPrice: Double,
    val totalAmount: Double
)

