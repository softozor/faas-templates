const { HttpError } = require('http-errors')

exports.handler = fn => {
  const handler = async (req, res) => {
    const input = req.body.input
    try {
      const result = await fn(input)
      return res.json(result)
    } catch (error) {
      console.error(`Caught error: ${error}`)
      if (error instanceof HttpError) {
        return res.status(error.status).json({
          code: error.name,
          message: error.message
        })
        } else {
          return res.status(400).json({
            code: error.name,
            message: error.message
        })
      }
    }
  }
  return handler
}