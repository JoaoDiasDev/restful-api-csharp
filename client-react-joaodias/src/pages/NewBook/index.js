import React, { useEffect, useMemo, useState } from "react";
import "./styles.css";
import logoImage from "../../assets/logo.png";
import { Link, useNavigate, useParams } from "react-router-dom";
import { FiArrowLeft } from "react-icons/fi";
import Api from "../../services/api";

export default function NewBook() {
  const [id, setId] = useState(null);
  const [author, setAuthor] = useState("");
  const [title, setTitle] = useState("");
  const [launchDate, setLaunchDate] = useState("");
  const [price, setPrice] = useState("");

  const { bookId } = useParams();

  const navigate = useNavigate();

  const accessToken = localStorage.getItem("accessToken");

  const authorization = useMemo(
    () => ({
      headers: {
        Authorization: `Bearer ${accessToken}`,
      },
    }),
    [accessToken]
  );

  useEffect(() => {
    async function fetchBookDetails() {
      if (bookId === "0") return;

      try {
        const response = await Api.get(`/api/v1/book/${bookId}`, authorization);
        const { id, title, author, price, launchDate } = response.data;
        const adjustedDate = launchDate.split("T", 10)[0];

        setId(id);
        setTitle(title);
        setAuthor(author);
        setPrice(price);
        setLaunchDate(adjustedDate);
      } catch (error) {
        console.error("Error fetching Book:", error);
      }
    }

    fetchBookDetails();
  }, [
    bookId,
    authorization,
    setId,
    setTitle,
    setAuthor,
    setPrice,
    setLaunchDate,
  ]);

  async function saveOrUpdate(e) {
    e.preventDefault();

    const data = {
      author,
      title,
      launchDate,
      price,
    };

    try {
      if (bookId === "0") {
        await Api.post("api/v1/book", data, authorization);
      } else {
        data.id = id;
        await Api.put("api/v1/book", data, authorization);
      }
    } catch (error) {
      alert("Error while recording Book! Try Again!");
    }
    navigate("/books");
  }

  return (
    <div className="new-book-container">
      <div className="content">
        <section className="form">
          <img src={logoImage} alt="JoaoDiasDev" />
          <h1>{bookId === "0" ? "Add New" : "Update"} Book</h1>
          <p>
            Enter the book information and click on{" "}
            {bookId === "0" ? "Add" : "Update"}!
          </p>
          <Link className="back-link" to="/books">
            <FiArrowLeft size={16} color="#251FC5" />
            Back to Books
          </Link>
        </section>
        <form onSubmit={saveOrUpdate}>
          <input
            placeholder="Title"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
          />
          <input
            placeholder="Author"
            value={author}
            onChange={(e) => setAuthor(e.target.value)}
          />
          <input
            type="date"
            value={launchDate}
            onChange={(e) => setLaunchDate(e.target.value)}
          />

          <input
            placeholder="Price"
            value={price}
            onChange={(e) => setPrice(e.target.value)}
          />

          <button className="button" type="submit">
            {bookId === "0" ? "Add" : "Update"}
          </button>
        </form>
      </div>
    </div>
  );
}
