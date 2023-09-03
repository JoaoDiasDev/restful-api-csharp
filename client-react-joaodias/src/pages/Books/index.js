import React, { useState, useEffect } from "react";
import "./styles.css";
import logoImage from "../../assets/logo.png";
import { Link, useNavigate } from "react-router-dom";
import { FiPower, FiEdit, FiTrash2 } from "react-icons/fi";
import Api from "../../services/api";

export default function Books() {
  const [books, setBooks] = useState([]);

  const userName = localStorage.getItem("userName");
  const accessToken = localStorage.getItem("accessToken");

  const navigate = useNavigate();

  useEffect(() => {
    async function fetchBooks() {
      try {
        const response = await Api.get("api/v1/book/asc/20/1", {
          headers: {
            Authorization: `Bearer ${accessToken}`,
          },
        });
        setBooks(response.data.list);
      } catch (error) {
        console.error("Error fetching books:", error);
      }
    }

    fetchBooks();
  }, [accessToken]);

  async function deleteBook(id) {
    try {
      await Api.delete(`api/v1/book/${id}`, {
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
      });

      setBooks(books.filter((book) => book.id !== id));
    } catch (error) {
      alert("Delete failed! Try again!");
    }
  }

  async function logout() {
    try {
      await Api.get(`api/v1/auth/revoke`, {
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
      });

      localStorage.clear();
      navigate("/");
    } catch (error) {
      alert("Logout failed! Try again!");
    }
  }

  return (
    <div className="book-container">
      <header>
        <img src={logoImage} alt="JoaoDiasDev" />
        <span>
          Welcome,{" "}
          <strong>{userName ? userName.toUpperCase() : "Guest"}</strong>!
        </span>
        <Link className="button" to="/book/new">
          Add New Book
        </Link>
        <button onClick={() => logout()} type="button">
          <FiPower size={18} color="#251FC5" />
        </button>
      </header>
      <h1>Registered Books</h1>
      <ul>
        {books.map((book) => (
          <li key={book.id}>
            <strong>Title:</strong>
            <p>{book.title}</p>
            <strong>Author:</strong>
            <p>{book.author}</p>
            <strong>Price:</strong>
            <p>
              {Intl.NumberFormat("pt-BR", {
                style: "currency",
                currency: "BRL",
              }).format(book.price)}
            </p>
            <strong>Release Date:</strong>
            <p>
              {Intl.DateTimeFormat("pt-BR").format(new Date(book.launchDate))}
            </p>

            <button type="button">
              <FiEdit size={20} color="#251FC5" />
            </button>

            <button onClick={() => deleteBook(book.id)} type="button">
              <FiTrash2 size={20} color="#251FC5" />
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
}
