using System.Collections.Generic;
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using QuizService.Model;
using QuizService.Model.Domain;
using System.Linq;
using QuizService.Data;
using QuizService.Services.Interfaces;
using QuizService.Exceptions;
using System;
using System.Net;

namespace QuizService.Controllers;

[Route("api/quizzes")]
public class QuizController : Controller
{
    private readonly IQuizService quizService;
    private readonly IQuestionService questionService;
    private readonly IAnswerService answerService;

    public QuizController(IQuizService quizService, IQuestionService questionService, IAnswerService answerService)
    {
        this.quizService = quizService;
        this.questionService = questionService;
        this.answerService = answerService;
    }

    // GET api/quizzes
    [HttpGet]
    public IActionResult Get()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(this.quizService.GetQuizzes());
    }

    // GET api/quizzes/5
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var quiz = this.quizService.GetQuiz(id);
            return Ok(quiz);
        }
        catch(NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch(Exception ex)
        {
            return BadRequest(new { message = "An unexpected error occurred." });
        }

    }

    // POST api/quizzes
    [HttpPost]
    public IActionResult Post([FromBody] QuizCreateModel value)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var quiz = this.quizService.AddQuiz(value);
        var locationUrl = $"/api/quizzes/{quiz.Id}";
        return Created(locationUrl,quiz);
    }

    // PUT api/quizzes/5
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] QuizUpdateModel value)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var quiz = this.quizService.UpdateQuiz(id, value);
            return Ok(quiz);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "An unexpected error occurred." });
        }
    }

    // DELETE api/quizzes/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var quiz = this.quizService.RemoveQuiz(id);
            return Ok(quiz);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "An unexpected error occurred." });
        }
    }

    // POST api/quizzes/5/questions
    [HttpPost]
    [Route("{id}/questions")]
    public IActionResult PostQuestion(int id, [FromBody] QuestionCreateModel value)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var question = this.questionService.AddQuestion(id,value);
            var locationUrl = $"/api/quizzes/{id}/questions/{question.Id}";
            return Created(locationUrl, question);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "An unexpected error occurred." });
        }
    }

    // PUT api/quizzes/5/questions/6
    [HttpPut("{id}/questions/{qid}")]
    public IActionResult PutQuestion(int id, int qid, [FromBody] QuestionUpdateModel value)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var question = this.questionService.UpdateQuestion(id,qid, value);
            return Ok(question);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (CustomException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "An unexpected error occurred." });
        }
    }

    // DELETE api/quizzes/5/questions/6
    [HttpDelete]
    [Route("{id}/questions/{qid}")]
    public IActionResult DeleteQuestion(int id, int qid)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var question = this.questionService.RemoveQuestion(id, qid);
            return Ok(question);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "An unexpected error occurred." });
        }
    }

    // POST api/quizzes/5/questions/6/answers
    [HttpPost]
    [Route("{id}/questions/{qid}/answers")]
    public IActionResult PostAnswer(int id, int qid, [FromBody] AnswerCreateModel value)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var answer = this.answerService.AddAnswer(qid, value);
            var locationUrl = $"/api/quizzes/{id}/questions/{qid}/answers/{answer.Id}";
            return Created(locationUrl, answer);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (CustomException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "An unexpected error occurred." });
        }
    }

    // PUT api/quizzes/5/questions/6/answers/7
    [HttpPut("{id}/questions/{qid}/answers/{aid}")]
    public IActionResult PutAnswer(int id, int qid, int aid, [FromBody] AnswerUpdateModel value)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var question = this.answerService.UpdateAnswer(id,qid,aid,value);
            return Ok(question);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (CustomException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "An unexpected error occurred." });
        }
    }

    // DELETE api/quizzes/5/questions/6/answers/7
    [HttpDelete]
    [Route("{id}/questions/{qid}/answers/{aid}")]
    public IActionResult DeleteAnswer(int id, int qid, int aid)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var question = this.answerService.RemoveAnswer(qid, aid);
            return Ok(question);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "An unexpected error occurred." });
        }
    }
}