package com.ead.eadapp

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView

class OrderAdapter(private var orders: List<Order>) : RecyclerView.Adapter<OrderAdapter.OrderViewHolder>() {

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): OrderViewHolder {
        val view = LayoutInflater.from(parent.context).inflate(R.layout.order_item, parent, false)
        return OrderViewHolder(view)
    }

    override fun onBindViewHolder(holder: OrderViewHolder, position: Int) {
        val order = orders[position]
        holder.bind(order)
    }

    override fun getItemCount(): Int = orders.size

    fun updateOrders(newOrders: List<Order>) {
        orders = newOrders
        notifyDataSetChanged()
    }

    class OrderViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        private val orderIdTextView: TextView = itemView.findViewById(R.id.orderIdTextView)
        private val statusTextView: TextView = itemView.findViewById(R.id.statusTextView)
        private val orderDateTextView: TextView = itemView.findViewById(R.id.orderDateTextView)

        fun bind(order: Order) {
            orderIdTextView.text = "Order ID: ${order.orderID}"
            statusTextView.text = "Status: ${order.status}"
            orderDateTextView.text = "Order Date: ${order.orderDate}"
        }
    }
}
