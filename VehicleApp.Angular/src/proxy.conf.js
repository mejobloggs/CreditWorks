const PROXY_CONFIG = [
  {
    context: [
      "/vehicles",
    ],
    target: "https://localhost:7259",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
