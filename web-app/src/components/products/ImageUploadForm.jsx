import React, { useState } from "react";
import axios from "axios";

const ImageUploadForm = () => {
  const [selectedFile, setSelectedFile] = useState(null);
  const [uploadResult, setUploadResult] = useState(null);
  const [deletePublicId, setDeletePublicId] = useState("");
  const [deleteMessage, setDeleteMessage] = useState("");

  // Handle file selection
  const handleFileChange = (event) => {
    setSelectedFile(event.target.files[0]);
  };

  // Handle image upload
  const handleUpload = async (event) => {
    event.preventDefault();
    if (!selectedFile) {
      alert("Please select a file to upload");
      return;
    }

    const formData = new FormData();
    formData.append("file", selectedFile);

    try {
      const response = await axios.post(
        "http://localhost:5271/api/image/upload",
        formData,
        {
          headers: { "Content-Type": "multipart/form-data" },
        }
      );

      setUploadResult(response.data);
      setSelectedFile(null); // Reset file input after successful upload
      alert("Image uploaded successfully");
    } catch (error) {
      console.error("Error uploading image", error);
      alert("Error uploading image");
    }
  };

  // Handle image deletion
  const handleDelete = async (event) => {
    event.preventDefault();
    if (!deletePublicId) {
      alert("Please enter a public ID to delete");
      return;
    }

    try {
      const response = await axios.delete(
        `http://localhost:5271/api/image/delete/${deletePublicId}`
      );
      setDeleteMessage(response.data.message);
      setDeletePublicId("");
    } catch (error) {
      console.error("Error deleting image", error);
      alert("Error deleting image");
    }
  };

  return (
    <div>
      <h2>Upload Image</h2>
      <form onSubmit={handleUpload}>
        <input type="file" onChange={handleFileChange} />
        <button type="submit">Upload</button>
      </form>

      {uploadResult && (
        <div>
          <h4>Upload Result</h4>
          <p>Public ID: {uploadResult.publicId}</p>
          <p>
            URL:{" "}
            <a href={uploadResult.url} target="_blank" rel="noreferrer">
              {uploadResult.url}
            </a>
          </p>
        </div>
      )}

      <h2>Delete Image</h2>
      <form onSubmit={handleDelete}>
        <input
          type="text"
          value={deletePublicId}
          onChange={(e) => setDeletePublicId(e.target.value)}
          placeholder="Enter Public ID"
        />
        <button type="submit">Delete</button>
      </form>

      {deleteMessage && <p>{deleteMessage}</p>}
    </div>
  );
};

export default ImageUploadForm;
