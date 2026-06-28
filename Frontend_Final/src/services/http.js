const BASE_URL = 'https://localhost:7233/api'

async function request(url, options = {}) {
  const res = await fetch(`${BASE_URL}${url}`, {
    headers: {
      'Content-Type': 'application/json',
      ...(options.headers || {})
    },
    ...options
  })

  const text = await res.text() // SOLO UNA VEZ

  let data = null

  try {
    data = text ? JSON.parse(text) : null
  } catch {
    data = { mensaje: text }
  }

  if (!res.ok) {
    throw {
      mensajes:
        data?.mensajes ??
        (data?.mensaje ? [data.mensaje] : ['Error inesperado'])
    }
  }

  return data
}

export const http = {
  get: (url) => request(url),
  post: (url, body) =>
    request(url, {
      method: 'POST',
      body: JSON.stringify(body)
    }),
  patch: (url, body) =>
    request(url, {
      method: 'PATCH',
      body: JSON.stringify(body)
    }),
  delete: (url) =>
    request(url, {
      method: 'DELETE'
    })
}